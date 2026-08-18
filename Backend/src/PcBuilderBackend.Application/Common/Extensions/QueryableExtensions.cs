using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderBy");

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderByDescending");

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "ThenBy");

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "ThenByDescending");
    
    public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        => ApplyOrder(source.AsQueryable(), propertyName, "OrderBy").AsEnumerable() as IOrderedEnumerable<T>
           ?? throw new InvalidOperationException();

    public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        => ApplyOrder(source.AsQueryable(), propertyName, "OrderByDescending").AsEnumerable() as IOrderedEnumerable<T>
           ?? throw new InvalidOperationException();

    public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string propertyName)
        => ApplyOrder(source.AsQueryable(), propertyName, "ThenBy").AsEnumerable() as IOrderedEnumerable<T>
           ?? throw new InvalidOperationException();

    public static IOrderedEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> source, string propertyName)
        => ApplyOrder(source.AsQueryable(), propertyName, "ThenByDescending").AsEnumerable() as IOrderedEnumerable<T>
           ?? throw new InvalidOperationException();
    
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> source,
        IReadOnlyList<string> sortBy,
        string sortDirection = "asc")
    {
        if (sortBy.Count == 0)
            return source;

        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        IOrderedQueryable<T>? ordered = null;

        for (var i = 0; i < sortBy.Count; i++)
        {
            var property = sortBy[i];
            if (string.IsNullOrWhiteSpace(property))
                continue;

            if (ordered is null)
            {
                ordered = descending
                    ? source.OrderByDescending(property)
                    : source.OrderBy(property);
            }
            else
            {
                ordered = descending
                    ? ordered.ThenByDescending(property)
                    : ordered.ThenBy(property);
            }
        }

        return ordered ?? source;
    }

    public static IEnumerable<T> ApplySorting<T>(
        this IEnumerable<T> source,
        IReadOnlyList<string> sortBy,
        string sortDirection = "asc")
    {
        if (sortBy.Count == 0)
            return source;

        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        IOrderedEnumerable<T>? ordered = null;

        foreach (var property in sortBy)
        {
            if (string.IsNullOrWhiteSpace(property))
                continue;

            if (ordered is null)
            {
                ordered = descending
                    ? source.OrderByDescending(property)
                    : source.OrderBy(property);
            }
            else
            {
                ordered = descending
                    ? ordered.ThenByDescending(property)
                    : ordered.ThenBy(property);
            }
        }

        return ordered ?? source;
    }

    public static async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        int pageIndex,
        int pageSize,
        IConfigurationProvider configuration,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ProjectTo<TDestination>(configuration)
            .ToListAsync(cancellationToken);

        return new PagedResult<TDestination>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(
        IQueryable<T> source,
        string propertyName,
        string methodName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression propertyAccess = parameter;

        // Support nested properties: "Address.City", "User.Profile.Name"
        foreach (var member in propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            propertyAccess = Expression.PropertyOrField(propertyAccess, member);
        }

        var lambda = Expression.Lambda(propertyAccess, parameter);
        var resultType = propertyAccess.Type;

        var method = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == methodName
                         && m.IsGenericMethodDefinition
                         && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), resultType);

        var result = method.Invoke(null, [source, lambda])!;
        return (IOrderedQueryable<T>)result;
    }
}
