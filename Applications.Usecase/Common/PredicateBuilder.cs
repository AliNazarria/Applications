using Applications.Usecase.Common.Models;
using ErrorOr;
using SharedKernel;
using System.Linq.Expressions;
using System.Reflection;
using OperationType = Applications.Usecase.Common.Models.OperationType;

namespace Applications.Usecase.Common;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() { return f => true; }
    public static Expression<Func<T, bool>> False<T>() { return f => false; }
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }
    public static ErrorOr<Expression<Func<T, bool>>> MakePredicate<T>(List<FilterDTO> filters)
    {
        if (!filters?.Any() ?? true)
            filters = [];
        if (filters.Any(f => f == null && string.IsNullOrWhiteSpace(f.Key)))
            return Errors.FilterInvalid();

        filters.Add(new FilterDTO(nameof(Entity.Deleted), "false", OperationType.Equal));
        try
        {
            var item = Expression.Parameter(typeof(T), "item");
            var body = filters.Where(f => f.Value != null).Select(f => MakePredicate(item, f)).Aggregate(Expression.And);
            var predicate = Expression.Lambda<Func<T, bool>>(body, item);
            return predicate;
        }
        catch (Exception ex)
        {
            return Errors.FilterInvalid();
        }
    }

    private static Expression MakePredicate(ParameterExpression item, FilterDTO filter)
    {
        MemberExpression member = Expression.Property(item, filter.Key);
        ConstantExpression constant = Expression.Constant(filter.Value);

        switch (filter.Operation)
        {
            case OperationType.Equal:
                return Expression.Equal(member, ConvetValueType(member, filter.Value));
            case OperationType.NotEqual:
                return Expression.NotEqual(member, ConvetValueType(member, filter.Value));
            case OperationType.Greater:
                return Expression.GreaterThan(member, ConvetValueType(member, filter.Value));
            case OperationType.GreaterEqual:
                return Expression.GreaterThanOrEqual(member, ConvetValueType(member, filter.Value));
            case OperationType.Less:
                return Expression.LessThan(member, ConvetValueType(member, filter.Value));
            case OperationType.LessEqual:
                return Expression.LessThanOrEqual(member, ConvetValueType(member, filter.Value));
            case OperationType.Like:
                return Expression.Call(member, containsMethod, constant);
            case OperationType.StartsWith:
                return Expression.Call(member, startsWithMethod, constant);
            case OperationType.EndsWith:
                return Expression.Call(member, endsWithMethod, constant);
        }

        return null;
    }
    private static ConstantExpression ConvetValueType(MemberExpression member, object value)
    {
        if (member.Type == typeof(int))
        {
            value = int.Parse(value.ToString());
        }
        else if (member.Type == typeof(decimal))
        {
            value = decimal.Parse(value.ToString());
        }
        else if (member.Type == typeof(float))
        {
            value = float.Parse(value.ToString());
        }
        else if (member.Type == typeof(double))
        {
            value = double.Parse(value.ToString());
        }
        else if (member.Type == typeof(bool))
        {
            value = bool.Parse(value.ToString());
        }
        else if (member.Type == typeof(DateTime))
        {
            value = DateTime.Parse(value.ToString());
        }

        return Expression.Constant(value);
    }
    private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
    private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
    private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
}
