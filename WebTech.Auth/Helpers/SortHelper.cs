using System.Linq.Expressions;

namespace WebTech.Auth.Helpers;

public static class SortHelper
{
    public static IOrderedQueryable<T> GetSorted<T>(IQueryable<T> query, string sortCriteria)
    {
        var sortInstructions = ParseSortCriteria(sortCriteria);
        IOrderedQueryable<T> orderedQuery = null;
        bool firstSortApplied = false;

        foreach (var (propertyName, descending) in sortInstructions)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            if (!firstSortApplied)
            {
                var methodName = descending ? "OrderByDescending" : "OrderBy";
                var orderByExpression = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), property.Type },
                    query.Expression,
                    Expression.Quote(lambda));

                orderedQuery = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(orderByExpression);
                firstSortApplied = true;
            }
            else
            {
                var methodName = descending ? "ThenByDescending" : "ThenBy";
                var orderByExpression = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), property.Type },
                    orderedQuery.Expression,
                    Expression.Quote(lambda));

                orderedQuery = (IOrderedQueryable<T>)orderedQuery.Provider.CreateQuery<T>(orderByExpression);
            }
        }

        return orderedQuery ?? (IOrderedQueryable<T>)query;
    }

    private static List<(string PropertyName, bool Descending)> ParseSortCriteria(string sortCriteria)
    {
        var sortInstructions = new List<(string PropertyName, bool Descending)>();
        var parts = sortCriteria.Split('~');

        for (var i = 1; i < parts.Length; i++)
        {
            var descending = parts[0].Equals("Desc", StringComparison.OrdinalIgnoreCase);
            sortInstructions.Add((parts[i], descending));
        }

        return sortInstructions;
    }
}