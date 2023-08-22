using PantryOrganizer.Application.Extensions;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public abstract class AbstractSorter<TSorting, TData> : ISorter<TSorting, TData>
    where TSorting : class
    where TData : class
{
    private readonly IList<ISorterRule<TSorting, TData>> rules
        = new List<ISorterRule<TSorting, TData>>();

    protected ISorterRuleBuilder<TSorting> SortBy<TProperty>(
        Expression<Func<TData, TProperty>> selector)
    {
        var rule = new SorterRule<TProperty>(selector);
        rules.Add(rule);
        return rule;
    }

    public IOrderedQueryable<TData> Apply(IQueryable<TData> query, TSorting? sortingInput)
    {
        if (sortingInput != default)
        {
            var sortings = GetSortings(sortingInput)
                .OrderBy(x => x.Parameter.Priority).ToList();

            if (sortings.Any())
                return ApplySortings(query, sortings);
        }

        var defaultSortings = rules.SelectWhere(
            rule => rule.GetDefault(),
            (_, parameter) => parameter != default && parameter.IsEnabled,
            (rule, parameter) => (rule, parameter!));

        return defaultSortings.Any()
            ? ApplySortings(query, defaultSortings)
            : throw new InvalidOperationException(
                "A default sorting or at least one sorting parameter has to be provided.");
    }

    private IEnumerable<(ISorterRule<TSorting, TData>, SortingParameter Parameter)> GetSortings(
        TSorting sortingInput)
    {
        foreach (var rule in rules)
        {
            var parameter = rule.GetParameter(sortingInput);

            if (parameter != default && parameter.IsEnabled)
                yield return (rule, parameter);
        }
    }

    private static IOrderedQueryable<TData> ApplySortings(
        IQueryable<TData> query,
        IEnumerable<(ISorterRule<TSorting, TData>, SortingParameter)> sortings)
    {
        (var firstRule, var firstParameter) = sortings.First();
        var orderedQuery = firstRule.Apply(query, firstParameter);

        foreach ((var rule, var parameter) in sortings.Skip(1))
            orderedQuery = rule.Apply(orderedQuery, parameter);

        return orderedQuery;
    }

    public class SorterRule<TProperty> :
        ISorterRuleBuilder<TSorting>,
        ISorterRule<TSorting, TData>
    {
        private readonly Expression<Func<TData, TProperty>> dataSelector;
        private Expression<Func<TSorting, SortingParameter?>> sortingSelector
            = x => new(default, default);
        private SortingParameter? defaultParameter;

        public SorterRule(Expression<Func<TData, TProperty>> selector)
            => dataSelector = selector;

        public SortingParameter? GetParameter(TSorting sortingInput)
            => sortingSelector.Compile()(sortingInput);

        public SortingParameter? GetDefault()
            => defaultParameter;

        public IOrderedQueryable<TData> Apply(
            IQueryable<TData> query,
            SortingParameter parameter)
            => parameter.Direction switch
            {
                SortingDirection.Ascending => query.OrderBy(dataSelector),
                SortingDirection.Descending => query.OrderByDescending(dataSelector),
                _ => throw new InvalidOperationException(
                    $"Sorting direction {parameter.Direction} is not valid.")
            };

        public IOrderedQueryable<TData> Apply(
            IOrderedQueryable<TData> query,
            SortingParameter parameter)
            => parameter.Direction switch
            {
                SortingDirection.Ascending => query.ThenBy(dataSelector),
                SortingDirection.Descending => query.ThenByDescending(dataSelector),
                _ => throw new InvalidOperationException(
                    $"Sorting direction {parameter.Direction} is not valid.")
            };

        public ISorterRuleBuilder<TSorting> Using(
            Expression<Func<TSorting, SortingParameter?>> selector)
        {
            sortingSelector = selector;
            return this;
        }

        public ISorterRuleBuilder<TSorting> AsDefault(
            SortingDirection direction = SortingDirection.Ascending,
            int priority = 0)
        {
            defaultParameter = new SortingParameter(direction, priority);
            return this;
        }
    }
}
