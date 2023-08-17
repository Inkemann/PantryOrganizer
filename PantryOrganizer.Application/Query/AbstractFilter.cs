using PantryOrganizer.Application.Extensions;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public abstract class AbstractFilter<TFilter, TData> : IFilter<TFilter, TData>
    where TFilter : class
    where TData : class
{
    private readonly IList<IFilterRule<TFilter, TData>> rules =
        new List<IFilterRule<TFilter, TData>>();

    protected IFilterRuleBuilder<TProperty, TData> FilterFor<TProperty>(
        Expression<Func<TFilter, TProperty?>> selector)
    {
        if (selector == null)
            throw new ArgumentNullException(nameof(selector));

        var rule = new FilterRule<TFilter, TProperty, TData>(selector);
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
}

public class FilterRule<TFilter, TProperty, TData> :
    IFilterRuleBuilder<TProperty, TData>,
    IFilterRule<TFilter, TData>
{
    protected Expression<Func<TFilter, TProperty?>> Selector { get; }
    protected Expression<Func<TProperty?, bool>> Condition { get; set; } = EmptyCondition();
    protected Expression<Func<TProperty?, TData, bool>> Filter { get; set; } = (x, y) => true;

    public FilterRule(Expression<Func<TFilter, TProperty?>> selector)
        => Selector = selector ?? throw new ArgumentNullException(nameof(selector));

    public IQueryable<TData> Apply(IQueryable<TData> query, TFilter filterInput)
    {
        var filterValue = Selector.Compile()(filterInput);

        if (Condition.Compile()(filterValue))
        {
            var curriedFilter = Filter.ApplyPartial(filterValue);
            query = query.Where(curriedFilter);
        }

        return query;
    }

    public IFilterRuleBuilder<TProperty, TData> When(
        Expression<Func<TProperty?, bool>> condition)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        return this;
    }

    public IFilterRuleBuilder<TProperty, TData> Predicate(
        Expression<Func<TProperty?, TData, bool>> filter)
    {
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        return this;
    }

    private static Expression<Func<TProperty?, bool>> EmptyCondition()
        => typeof(TProperty) == typeof(string) ?
            (value => !string.IsNullOrEmpty(value as string)) :
            (value => EqualityComparer<TProperty>.Default.Equals(value, default(TProperty)));
}
