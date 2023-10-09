using System.Linq.Expressions;

namespace WebTech.Auth.Helpers;

public static class SortHelper
{
    public static IOrderedQueryable<T> GetSorted<T>(IQueryable<T> query, string sortCriteria)
    {
        var (propertyName, descending) = ParseSortCriteria(sortCriteria);
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodname = descending ? "OrderByDescending" : "OrderBy";
        var orderByExpression = Expression.Call(
            typeof(Queryable),
            methodname,
            new[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(orderByExpression);
    }

    public static (string PropertyName, bool Descending) ParseSortCriteria(string sortCriteria)
    {
        var parts = sortCriteria.Split('~');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid sort criteria format");
        }

        var propertyName = parts[1];
        var descending = parts[0].Equals("Desc", StringComparison.OrdinalIgnoreCase);

        return (propertyName, descending);
    }
}