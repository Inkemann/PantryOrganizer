using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public interface IFilter<TFilter, TData>
{
    public IQueryable<TData> Apply(IQueryable<TData> query, TFilter? filterInput);
}

public interface IFilterRuleBuilder<TProperty, TData>
{
    public IFilterRuleBuilder<TProperty, TData> Predicate(
        Expression<Func<TProperty?, TData, bool>> filter);

    public IFilterRuleBuilder<TProperty, TData> When(Expression<Func<TProperty?, bool>> condition);
}

public interface IFilterRule<TFilter, TData>
{
    internal IQueryable<TData> Apply(IQueryable<TData> query, TFilter filterInput);
}
