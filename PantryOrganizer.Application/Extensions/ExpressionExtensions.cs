using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace PantryOrganizer.Application.Extensions;

internal static class ExpressionExtensions
{
    public static Expression<Func<T2, TResult>> ApplyPartial<T1, T2, TResult>(
        this Expression<Func<T1, T2, TResult>> expression,
        T1 value)
    {
        var parameter = expression.Parameters[0];
        var constant = Expression.Constant(value, parameter.Type);
        var visitor = new ReplacementVisitor(parameter, constant);
        var newBody = visitor.Visit(expression.Body);
        return Expression.Lambda<Func<T2, TResult>>(newBody, expression.Parameters[1]);
    }

    public static T ReplaceExpression<T>(
        this T expression,
        Expression original,
        Expression replacement)
        where T : Expression
    {
        var replacer = new ReplacementVisitor(original, replacement);
        return replacer.VisitAndConvert(expression, nameof(ReplaceExpression));
    }

    internal class ReplacementVisitor : ExpressionVisitor
    {
        private readonly Expression original, replacement;

        public ReplacementVisitor(Expression original, Expression replacement)
        {
            this.original = original;
            this.replacement = replacement;
        }

        [return: NotNullIfNotNull(nameof(node))]
        public override Expression? Visit(Expression? node)
            => node == original ? replacement : base.Visit(node);
    }
}
