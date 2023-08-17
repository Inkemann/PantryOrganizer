namespace PantryOrganizer.Application.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TResult> SelectWhere<TSource, TValue, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TValue> valueSelector,
            Func<TSource, TValue, bool> predicate,
            Func<TSource, TValue, TResult> resultSelector)
    {
        foreach (var item in source)
        {
            var value = valueSelector(item);
            if (predicate(item, value))
                yield return resultSelector(item, value);
        }
    }
}
