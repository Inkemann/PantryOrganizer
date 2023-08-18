using PantryOrganizer.Application.Extensions;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public abstract class AbstractFilter<TFilter, TData> :
    IFilter<TFilter, TData>,
    IFilterBuilder<TFilter, TData>
    where TFilter : class
    where TData : class
{
    private readonly IList<IFilterRule<TFilter, TData>> rules
        = new List<IFilterRule<TFilter, TData>>();
    private readonly IDictionary<Type, Func<object?, bool>> defaultConditions
        = new Dictionary<Type, Func<object?, bool>>();

    public IFilterBuilder<TFilter, TData> Defaults => this;

    protected IFilterRuleBuilder<TFilter, TProperty> FilterFor<TProperty>(
        Expression<Func<TData, TProperty?>> selector)
    {
        var rule = new FilterRule<TProperty>(this, selector);
        rules.Add(rule);
        return rule;
    }

    public IQueryable<TData> Apply(IQueryable<TData> query, TFilter? filterInput)
    {
        if (filterInput == null)
            return query;

        foreach (var rule in rules)
            query = rule.Apply(query, filterInput);

        return query;
    }

    public IFilterBuilder<TFilter, TData> AddConditionForType<T>(Func<T?, bool> condition)
    {
        defaultConditions[typeof(T)] = value => condition((T?)value);
        return this;
    }

    public class FilterRule<TProperty> :
        IFilterRule<TFilter, TData>,
        IFilterRuleBuilder<TFilter, TProperty>
    {
        private readonly AbstractFilter<TFilter, TData> parentFilter;
        private readonly Expression<Func<TData, TProperty?>> dataSelector;
        private Expression<Func<TFilter, TProperty?>>? filterSelector;
        private Func<object?, bool>? condition = null;
        private Expression<Func<TProperty?, TProperty?, bool>> filter
            = (filterProperty, dataProperty) => true;

        public FilterRule(
            AbstractFilter<TFilter, TData> parentFilter,
            Expression<Func<TData, TProperty?>> dataSelector)
        {
            this.parentFilter = parentFilter;
            this.dataSelector = dataSelector;
        }

        public IQueryable<TData> Apply(IQueryable<TData> query, TFilter filterInput)
        {
            if (filterSelector == null)
                throw new InvalidOperationException();

            var filterValue = filterSelector.Compile()(filterInput);

            if ((condition ?? GetConditionFromDefaults())(filterValue))
            {
                var preparedFilter = PrepareFilter();
                var curriedFilter = preparedFilter.ApplyPartial(filterValue);
                return query.Where(curriedFilter);
            }

            return query;
        }

        public IFilterRuleBuilder<TFilter, TProperty> Using(
            Expression<Func<TFilter, TProperty?>> filterSelector)
        {
            this.filterSelector = filterSelector;
            return this;
        }

        public IFilterRuleBuilder<TFilter, TProperty> When(
            Func<TProperty?, bool> condition)
        {
            this.condition = value => condition((TProperty?)value);
            return this;
        }

        public IFilterRuleBuilder<TFilter, TProperty> Predicate(
            Expression<Func<TProperty?, TProperty?, bool>> filter)
        {
            this.filter = filter;
            return this;
        }

        private Expression<Func<TProperty?, TData, bool>> PrepareFilter()
        {
            var filterParameter = Expression.Parameter(typeof(TProperty?));
            var dataParameter = Expression.Parameter(typeof(TData));

            var dataBody = dataSelector.Body.ReplaceExpression(
                dataSelector.Parameters[0],
                dataParameter);
            var filterBody = filter.Body
                .ReplaceExpression(
                    filter.Parameters[0],
                    filterParameter)
                .ReplaceExpression(
                    filter.Parameters[1],
                    dataBody);

            return Expression.Lambda<Func<TProperty?, TData, bool>>(
                filterBody,
                filterParameter,
                dataParameter);
        }

        private Func<object?, bool> GetConditionFromDefaults()
        {
            var propertyType = typeof(TProperty);
            Func<object?, bool>? directTypeCondition = null;
            var inheritedConditions = new List<Func<object?, bool>>();

            foreach ((var type, var condition) in parentFilter.defaultConditions)
            {
                if (type == propertyType || type == Nullable.GetUnderlyingType(propertyType))
                    directTypeCondition = condition;
                else if (propertyType.IsAssignableFrom(type))
                    inheritedConditions.Add(condition);
            }

            return directTypeCondition
                ?? inheritedConditions.FirstOrDefault()
                ?? (value => true);
        }
    }
}
