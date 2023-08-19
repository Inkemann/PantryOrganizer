using PantryOrganizer.Application.Query;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Extensions;

public static class DefaultFilterExtensions
{
    public static IFilterBuilder<TFilter, TData> IgnoreNull<TFilter, TData>(
        this IFilterBuilder<TFilter, TData> filter)
        => filter.AddConditionForType<object>(value => value != null);

    public static IFilterBuilder<TFilter, TData> IgnoreDefault<TFilter, TData>(
        this IFilterBuilder<TFilter, TData> filter)
        => filter.AddConditionForType<object>(value => value != default);

    public static IFilterBuilder<TFilter, string> IgnoreEmpty<TFilter>(
        this IFilterBuilder<TFilter, string> filter)
        => filter.AddConditionForType<string>(value => !string.IsNullOrEmpty(value));

    public static IFilterBuilder<TFilter, string> IgnoreWhitespace<TFilter>(
        this IFilterBuilder<TFilter, string> filter)
        => filter.AddConditionForType<string>(value => !string.IsNullOrWhiteSpace(value));

    public static IFilterRuleBuilder<TFilter, TProperty> IgnoreNull<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule)
        => filterRule.When(value => value != null);

    public static IFilterRuleBuilder<TFilter, TProperty> IgnoreDefault<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule)
        => filterRule.When(value => !EqualityComparer<TProperty>.Default.Equals(value, default));

    public static IFilterRuleBuilder<TFilter, string> IgnoreEmpty<TFilter>(
        this IFilterRuleBuilder<TFilter, string> filterRule)
        => filterRule.When(value => !string.IsNullOrEmpty(value));

    public static IFilterRuleBuilder<TFilter, string> IgnoreWhitespace<TFilter>(
        this IFilterRuleBuilder<TFilter, string> filterRule)
        => filterRule.When(value => !string.IsNullOrWhiteSpace(value));

    public static IFilterRuleBuilder<TFilter, TProperty> Equals<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.Equal(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<TFilter, TProperty> GreaterThan<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.GreaterThan(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<TFilter, TProperty> GreaterThanOrEquals<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.GreaterThanOrEqual(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<TFilter, TProperty> LessThan<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.LessThan(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<TFilter, TProperty> LessThanOrEquals<TFilter, TProperty>(
        this IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector)
    {
        static Expression predicate(Expression dataParameter, Expression propertyParameter)
            => Expression.LessThanOrEqual(dataParameter, propertyParameter);

        return SetSelectorPredicate(filterRule, selector, predicate);
    }

    public static IFilterRuleBuilder<T, string> Contains<T>(
        this IFilterRuleBuilder<T, string> filterRule,
        Expression<Func<T, string?>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.Contains));

    public static IFilterRuleBuilder<T, string> StartsWith<T>(
        this IFilterRuleBuilder<T, string> filterRule,
        Expression<Func<T, string?>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.StartsWith));

    public static IFilterRuleBuilder<T, string> EndsWith<T>(
        this IFilterRuleBuilder<T, string> filterRule,
        Expression<Func<T, string?>> selector)
        => SetStringMethodPredicate(filterRule, selector, nameof(string.EndsWith));

    private static IFilterRuleBuilder<TFilter, string> SetStringMethodPredicate<TFilter>(
        IFilterRuleBuilder<TFilter, string> filterRule,
        Expression<Func<TFilter, string?>> selector,
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

    private static IFilterRuleBuilder<TFilter, TProperty> SetSelectorPredicate<TFilter, TProperty>(
        IFilterRuleBuilder<TFilter, TProperty> filterRule,
        Expression<Func<TFilter, TProperty?>> selector,
        Func<Expression, Expression, Expression> filterBuilder)
    {
        var predicate = BuildPredicate<TProperty>(filterBuilder);

        filterRule.Using(selector);
        filterRule.Predicate(predicate);
        return filterRule;
    }

    private static Expression<Func<T2?, T2?, bool>> BuildPredicate<T2>(
        Func<Expression, Expression, Expression> filterBuilder)
    {
        var dataParameter = Expression.Parameter(typeof(T2?));
        var propertyParameter = Expression.Parameter(typeof(T2?));
        var filter = filterBuilder(dataParameter, propertyParameter);

        return Expression.Lambda<Func<T2?, T2?, bool>>(
            filter,
            propertyParameter,
            dataParameter);
    }
}
