using PantryOrganizer.Application.Query;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Extensions;

public static class DefaultFilterExtensions
{
    public static IFilterRuleBuilder<T1, T2> Equals<T1, T2>(
        this IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.Equal(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<T1, T2> GreaterThan<T1, T2>(
        this IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.GreaterThan(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<T1, T2> GreaterThanEquals<T2, T1>(
        this IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.GreaterThanOrEqual(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<T1, T2> LessThan<T1, T2>(
        this IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.LessThan(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<T1, T2> LessThanEquals<T1, T2>(
        this IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.LessThanOrEqual(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<string, T> Contains<T>(
        this IFilterRuleBuilder<string, T> filterRule,
        Expression<Func<T, string>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.Contains));

    public static IFilterRuleBuilder<string, T> StartsWith<T>(
        this IFilterRuleBuilder<string, T> filterRule,
        Expression<Func<T, string>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.StartsWith));

    public static IFilterRuleBuilder<string, T> EndsWith<T>(
        this IFilterRuleBuilder<string, T> filterRule,
        Expression<Func<T, string>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.EndsWith));

    private static IFilterRuleBuilder<string, T> SetStringMethodPredicate<T>(
        IFilterRuleBuilder<string, T> filterRule,
        Expression<Func<T, string>> selector,
        string methodName)
    {
        var method = typeof(string).GetMethod(
            methodName,
            new[] { typeof(string) })
            ?? throw new MissingMethodException(typeof(string).FullName, methodName);
        Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.Call(dataParameter, method, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    private static IFilterRuleBuilder<T1, T2> SetSelectorPredicate<T1, T2>(
        IFilterRuleBuilder<T1, T2> filterRule,
        Expression<Func<T2, T1>> selector,
        Func<Expression, Expression, Expression> filterBuilder)
    {
        var predicate = BuildSelectorPredicate(selector, filterBuilder);

        filterRule.Predicate(predicate);
        return filterRule;
    }

    private static Expression<Func<T1?, T2, bool>> BuildSelectorPredicate<T1, T2>(
        Expression<Func<T2, T1>> selector,
        Func<Expression, Expression, Expression> filterBuilder)
    {
        var selectorParameter = Expression.Parameter(typeof(T2));
        var selectorReplaced = selector.Body.ReplaceExpressions(
            selector.Parameters.First(),
            selectorParameter);

        var dataParameter = Expression.Parameter(typeof(T1?));
        var propertyParameter = Expression.Parameter(typeof(T1?));
        var filter = filterBuilder(dataParameter, propertyParameter);

        var filterReplaced = filter.ReplaceExpressions(dataParameter, selectorReplaced);
        return Expression.Lambda<Func<T1?, T2, bool>>(
            filterReplaced,
            propertyParameter,
            selectorParameter);
    }
}
