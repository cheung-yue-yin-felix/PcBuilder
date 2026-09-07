using System.Linq.Expressions;
using System.Reflection;

namespace PcBuilderBackend.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderBy");

    public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        => Enumerable.OrderBy(source, CompileKeySelector<T>(propertyName));

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderByDescending");

    public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        => Enumerable.OrderByDescending(source, CompileKeySelector<T>(propertyName));

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "ThenBy");

    public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string propertyName)
        => Enumerable.ThenBy(source, CompileKeySelector<T>(propertyName));

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "ThenByDescending");

    public static IOrderedEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> source, string propertyName)
        => Enumerable.ThenByDescending(source, CompileKeySelector<T>(propertyName));
    
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<T> WhereIf<T, TValue>(
        this IQueryable<T> source,
        TValue? value,
        Func<TValue, Expression<Func<T, bool>>> predicateFactory)
        where TValue : class
    {
        return value is null ? source : source.Where(predicateFactory(value));
    }

    public static IQueryable<T> WhereIfHasText<T>(
        this IQueryable<T> source,
        string? value,
        Func<string, Expression<Func<T, bool>>> predicateFactory)
    {
        return string.IsNullOrWhiteSpace(value) ? source : source.Where(predicateFactory(value));
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

    private static Func<T, object?> CompileKeySelector<T>(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression access = parameter;

        foreach (var member in propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            var property = access.Type.GetProperty(
                member,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy)
                ?? throw new ArgumentException($"Property '{member}' was not found on {access.Type.Name}.");

            access = Expression.Property(access, property);
        }

        var boxed = Expression.Convert(access, typeof(object));
        return Expression.Lambda<Func<T, object?>>(boxed, parameter).Compile();
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
