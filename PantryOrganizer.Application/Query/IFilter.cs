using System.Linq.Expressions;

namespace PantryOrganizer.Application.Query;

public interface IFilter<TFilter, TData>
{
    public IQueryable<TData> Apply(IQueryable<TData> query, TFilter? filterInput);
}

public interface IFilterRule<TFilter, TData>
{
    internal IQueryable<TData> Apply(IQueryable<TData> query, TFilter filterInput);
}

public interface IFilterRuleBuilder<TFilter, TProperty>
{
    public IFilterRuleBuilder<TFilter, TProperty> Predicate(
        Expression<Func<TProperty?, TProperty?, bool>> filter);

    public IFilterRuleBuilder<TFilter, TProperty> Using(
        Expression<Func<TFilter, TProperty?>> filterSelector);

    public IFilterRuleBuilder<TFilter, TProperty> When(
        Expression<Func<TProperty?, bool>> condition);
}
