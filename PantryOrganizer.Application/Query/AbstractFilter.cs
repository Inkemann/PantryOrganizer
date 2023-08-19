using PantryOrganizer.Application.Extensions;
using System.Linq.Expressions;
using System.Reflection;

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
        var rule = new SingleFilterRule<TProperty>(this, selector);
        rules.Add(rule);
        return rule;
    }

    protected IFilterRuleBuilder<TFilter, TProperty> FilterForAny<TProperty>(
        Expression<Func<TData, IEnumerable<TProperty>>> selector)
    {
        var rule = new EnumerableFilterRule<TProperty>(
            this,
            selector,
            EnumerableFilterRule<TProperty>.EnumerableQuantifier.Any);
        rules.Add(rule);
        return rule;
    }

    protected IFilterRuleBuilder<TFilter, TProperty> FilterForAll<TProperty>(
        Expression<Func<TData, IEnumerable<TProperty>>> selector)
    {
        var rule = new EnumerableFilterRule<TProperty>(
            this,
            selector,
            EnumerableFilterRule<TProperty>.EnumerableQuantifier.All);
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

    private abstract class FilterRule<TProperty> :
        IFilterRule<TFilter, TData>,
        IFilterRuleBuilder<TFilter, TProperty>
    {
        protected readonly AbstractFilter<TFilter, TData> parentFilter;
        protected Expression<Func<TFilter, TProperty?>>? filterSelector;
        protected Func<object?, bool>? condition = null;
        protected Expression<Func<TProperty?, TProperty?, bool>> filter
            = (filterProperty, dataProperty) => true;

        public FilterRule(
            AbstractFilter<TFilter, TData> parentFilter)
            => this.parentFilter = parentFilter;

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

        protected abstract Expression<Func<TProperty?, TData, bool>> PrepareFilter();

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

    private class SingleFilterRule<TProperty> :
        FilterRule<TProperty>
    {
        private readonly Expression<Func<TData, TProperty?>> dataSelector;

        public SingleFilterRule(
            AbstractFilter<TFilter, TData> parentFilter,
            Expression<Func<TData, TProperty?>> dataSelector)
            : base(parentFilter)
            => this.dataSelector = dataSelector;

        protected override Expression<Func<TProperty?, TData, bool>> PrepareFilter()
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
    }

    private class EnumerableFilterRule<TProperty> :
        FilterRule<TProperty>
    {
        private readonly EnumerableQuantifier quantifier;
        private readonly Expression<Func<TData, IEnumerable<TProperty>>> dataSelector;

        public EnumerableFilterRule(
            AbstractFilter<TFilter, TData> parentFilter,
            Expression<Func<TData, IEnumerable<TProperty>>> dataSelector,
            EnumerableQuantifier quantifier)
            : base(parentFilter)
        {
            this.dataSelector = dataSelector;
            this.quantifier = quantifier;
        }

        protected override Expression<Func<TProperty?, TData, bool>> PrepareFilter()
        {
            var method = GetEnumerableMethod();

            var filterParameter = Expression.Parameter(typeof(TProperty?));
            var dataParameter = Expression.Parameter(typeof(TData));
            var enumerableItemParameter = Expression.Parameter(typeof(TProperty));

            var dataBody = dataSelector.Body.ReplaceExpression(
                dataSelector.Parameters[0],
                dataParameter);
            var filterBody = filter.Body
                .ReplaceExpression(
                    filter.Parameters[0],
                    filterParameter)
                .ReplaceExpression(
                    filter.Parameters[1],
                    enumerableItemParameter
                );

            var enumerableFilter = Expression.Lambda<Func<TProperty, bool>>(
                filterBody,
                enumerableItemParameter);

            var combinedFilter = Expression.Call(method, dataBody, enumerableFilter);

            return Expression.Lambda<Func<TProperty?, TData, bool>>(
                combinedFilter,
                filterParameter,
                dataParameter);
        }

        private MethodInfo GetEnumerableMethod()
        {
            string methodName = quantifier switch
            {
                EnumerableQuantifier.Any => nameof(Enumerable.Any),
                EnumerableQuantifier.All => nameof(Enumerable.All),
                _ => throw new InvalidOperationException(
                    $"The quantifier {quantifier} is not valid.")
            };

            var method = typeof(Enumerable)
                .GetMethods()
                .Single(methodInfo =>
                    methodInfo.Name == methodName
                    && methodInfo.GetParameters().Length == 2);
            return method.MakeGenericMethod(typeof(TProperty));
        }

        internal enum EnumerableQuantifier
        {
            Any,
            All,
        }
    }
}
