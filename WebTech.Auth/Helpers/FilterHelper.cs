﻿using System.Linq.Expressions;
using WebTech.Auth.Models.FilterModels;

namespace WebTech.Auth.Helpers;

public static class FilterHelper
{
    /// <summary>
    /// Applies filters to an IQueryable based on filter criteria.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IQueryable</typeparam>
    /// <param name="queryable">The IQueryable to filter.</param>
    /// <param name="filterCriteria">The filter criteria in string format.</param>
    /// <returns>An IQueryable with applied filters.</returns>
    public static IQueryable<T> GetFiltered<T>(IQueryable<T> queryable, string filterCriteria)
    {
        var filters = GetFilters(filterCriteria);
        var combinedExpressions = CombineExpressions<T>(filters, filterCriteria);
        
        return queryable.Where(combinedExpressions);
    }

    private static List<FilterDefinition> GetFilters(string conditions)
    {
        var parts = conditions.Split(new[] { "~and~", "~or~" }, StringSplitOptions.RemoveEmptyEntries);

        var filters =
            (from part in parts
             select part.Split('~')
                into elements
             where elements.Length == 3
             select new FilterDefinition
             {
                 Name = elements[0],
                 Operator = elements[1],
                 Value = elements[2]
             })
            .ToList();

        return filters;
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(
        List<FilterDefinition> filters, string conditions)
    {
        if(filters.Count == 0)
        {
            throw new ArgumentException("The 'filter' list must contain at least one filter");
        }

        if(filters.Count == 1)
        {
            return GetFilterExpression<T>(filters[0]);
        }

        var filterExpressions = filters.Select(GetFilterExpression<T>).ToArray();

        if (conditions.Contains("~or~"))
        {
            return filterExpressions.Aggregate((expr1, expr2) => expr1.Or(expr2));
        }
        else
        {
            return filterExpressions.Aggregate((expr1, expr2) => expr1.And(expr2));
        }
    }

    private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var and = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );
        var lambda = Expression.Lambda<Func<T, bool>>(and, param);
        return lambda;
    }

    private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var or = Expression.OrElse(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );
        var lambda = Expression.Lambda<Func<T, bool>>(or, param);
        return lambda;
    }

    private static Expression<Func<T, bool>> GetFilterExpression<T>(FilterDefinition filter)
    {
        var type = typeof(T);
        var param = Expression.Parameter(type, "x");
        var property = type.GetProperty(filter.Name);

        if (property == null)
            return x => true;

        var propertyAccess = GetPropertyExpression(param, filter.Name);
        var convertedConstant = ConvertToPropertyType(filter.Value, property.PropertyType);

        if (convertedConstant == null)
            throw new Exception("not supported type");

        Expression constant;

        if (filter.Value.Equals("null"))
        {
            if (!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null)
            {
                constant = Expression.Constant(null, property.PropertyType);
            }
            else
            {
                constant = Expression.Constant(false);
            }
        }
        else
        {
            constant = Expression.Constant(Convert.ChangeType(filter.Value, property.PropertyType), property.PropertyType);
        }

        Expression predicate = filter.Operator switch
        {
            "eq" => Expression.Equal(propertyAccess, constant),
            "ne" => Expression.NotEqual(propertyAccess, constant),
            "lt" => Expression.LessThan(propertyAccess, constant),
            "gt" => Expression.GreaterThan(propertyAccess, constant),
            "le" => Expression.LessThanOrEqual(propertyAccess, constant),
            "ge" => Expression.GreaterThanOrEqual(propertyAccess, constant),
            "sw" => Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, constant),
            "ew" => Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, constant),
            "con" => Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
            _ => throw new Exception("not supported operator")
        };

        return Expression.Lambda<Func<T, bool>>(predicate, param);
    }

    private static MemberExpression GetPropertyExpression(ParameterExpression param, string propertyName)
    {
        var parts = propertyName.Split('.');
        var propertyAccess = Expression.Property(param, parts[0]);

        for (int i = 1; i < parts.Length; i++)
        {
            propertyAccess = Expression.Property(propertyAccess, parts[i]);
        }

        return propertyAccess;
    }

    private static object ConvertToPropertyType(string value, Type propertyType)
    {
        if (propertyType == typeof(bool))
            return bool.Parse(value);

        if (propertyType == typeof(int))
            return int.Parse(value);

        if (propertyType == typeof(float))
            return float.Parse(value);

        if (propertyType == typeof(double))
            return double.Parse(value);

        if (propertyType == typeof(string))
            return (typeof(string), value);

        if (propertyType == typeof(byte))
            return byte.Parse(value);

        if (propertyType == typeof(char))
            return char.Parse(value);

        if (propertyType == typeof(decimal))
            return decimal.Parse(value);

        throw new ArgumentException("Unsupported property type: " + propertyType.FullName);
    }
}