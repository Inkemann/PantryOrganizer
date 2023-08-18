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
        if (selector == null)
            throw new ArgumentNullException(nameof(selector));

        var rule = new SorterRule<TProperty>(selector);
        rules.Add(rule);
        return rule;
    }

    public IOrderedQueryable<TData> Apply(IQueryable<TData> query, TSorting? sortingInput)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (sortingInput != null)
        {
            var sortings = new List<(ISorterRule<TSorting, TData>, SortingParameter Parameter)>();

            foreach (var rule in rules)
            {
                var parameter = rule.GetParameter(sortingInput);

                if (parameter != null && parameter.IsEnabled)
                    sortings.Add((rule, parameter));
            }

            sortings = sortings.OrderBy(x => x.Parameter.Priority).ToList();

            if (sortings.Count > 0)
                return AbstractSorter<TSorting, TData>.ApplySortings(query, sortings);
        }

        var defaultSortings = rules.SelectWhere(
            rule => rule.GetDefault(),
            (_, parameter) => parameter != null && parameter.IsEnabled,
            (rule, parameter) => (rule, parameter!));

        return defaultSortings.Any()
            ? AbstractSorter<TSorting, TData>.ApplySortings(query, defaultSortings)
            : throw new InvalidOperationException(
                "A default sorting or at least one sorting parameter has to be provided.");
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
            = x => new(null, default);
        private SortingParameter? defaultParameter;

        public SorterRule(Expression<Func<TData, TProperty>> selector)
            => dataSelector = selector ?? throw new ArgumentNullException(nameof(selector));

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
            sortingSelector = selector ?? throw new ArgumentNullException(nameof(selector));
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
