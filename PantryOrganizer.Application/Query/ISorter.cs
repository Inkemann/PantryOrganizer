using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public interface ISorter<TSorting, TData>
{
    public IOrderedQueryable<TData> Apply(IQueryable<TData> query, TSorting? sortingInput);
}

public interface ISorterRuleBuilder<TSorting>
{
    public ISorterRuleBuilder<TSorting> Using(
        Expression<Func<TSorting, SortingParameter?>> selector);

    public ISorterRuleBuilder<TSorting> AsDefault(
        SortingDirection direction = SortingDirection.Ascending,
        int priority = 0);
}

public interface ISorterRule<TSorting, TData>
{
    internal SortingParameter? GetParameter(TSorting sortingInput);

    internal SortingParameter? GetDefault();

    internal IOrderedQueryable<TData> Apply(
        IQueryable<TData> query,
        SortingParameter parameter);

    internal IOrderedQueryable<TData> Apply(
        IOrderedQueryable<TData> query,
        SortingParameter parameter);
}
