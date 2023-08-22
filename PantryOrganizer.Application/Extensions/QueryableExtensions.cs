using PantryOrganizer.Application.Query;

namespace PantryOrganizer.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TData> Filter<TFilter, TData>(
        this IQueryable<TData> query,
        IFilter<TFilter, TData> filter,
        TFilter? filterData)
        => filter.Apply(query, filterData);

    public static IOrderedQueryable<TData> Sort<TSorting, TData>(
        this IQueryable<TData> query,
        ISorter<TSorting, TData> sorter,
        TSorting? sortingData)
        => sorter.Apply(query, sortingData);

    public static IQueryable<TData> Paginate<TData>(
        this IQueryable<TData> query,
        IPagination? pagination)
        => pagination != default ?
            query.Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage) :
            query;
}
