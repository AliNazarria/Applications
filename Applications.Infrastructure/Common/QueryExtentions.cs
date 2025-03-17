using System.Linq.Expressions;
using System.Reflection;

namespace Applications.Infrastructure.Common;

public static class QueryExtentions
{
    public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return (IOrderedQueryable<TEntity>)query;

        var properties = orderBy.Split(',').Select(GetOrderByData).Where(x => !string.IsNullOrWhiteSpace(x.Item1));
        if (!properties?.Any() ?? true)
            return (IOrderedQueryable<TEntity>)query;

        var orderedQuery = query.OrderByProperty(properties.First().columnName, $"OrderBy{properties.First().sort}");
        foreach (var selector in properties.Skip(1))
            orderedQuery = orderedQuery.OrderByProperty(selector.columnName, $"ThenBy{selector.sort}");

        return orderedQuery;
    }
    private static (string columnName, string sort) GetOrderByData(string value)
    {
        var data = value.Split(' ');
        if (data.Length == 1)
            return new(data[0], "");
        return new(data[0], data[1].ToLower() == "asc" ? "" : "Descending");
    }
    public static IOrderedQueryable<TEntity> OrderByProperty<TEntity>(this IQueryable<TEntity> query, string propertyName, string methodName)
    {
        Type entityType = typeof(TEntity);
        ParameterExpression arg = Expression.Parameter(entityType, "x");
        MemberExpression property = Expression.Property(arg, propertyName);
        LambdaExpression selector = Expression.Lambda(property, new ParameterExpression[] { arg });
        MethodInfo method = typeof(Queryable).GetMethods()
             .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
             .Where(m => m.GetParameters().ToList().Count == 2)
             .Single();

        MethodInfo genericMethod = method.MakeGenericMethod(entityType, property.Type);
        return (IOrderedQueryable<TEntity>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
    }
}