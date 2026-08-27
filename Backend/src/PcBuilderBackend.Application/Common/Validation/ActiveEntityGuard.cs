using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Common.Validation;

public static class ActiveEntityGuard
{
    public static Task EnsureManufacturersExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountManufacturersAsync(set, ct),
            "Manufacturer", cancellationToken);

    public static Task EnsureSocketsExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountSocketsAsync(set, ct),
            "Socket", cancellationToken);

    public static Task EnsureChipsetsExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountChipsetsAsync(set, ct),
            "Chipset", cancellationToken);

    public static Task EnsureCpuSeriesExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountCpuSeriesAsync(set, ct),
            "CPU series", cancellationToken);

    public static Task EnsureGpuSeriesExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountGpuSeriesAsync(set, ct),
            "GPU series", cancellationToken);

    public static Task EnsureGpusExist(
        IActiveEntityLookup lookup, IEnumerable<Guid> ids, CancellationToken cancellationToken) =>
        EnsureExist(ids, (set, ct) => lookup.CountGpusAsync(set, ct),
            "GPU", cancellationToken);

    private static async Task EnsureExist(
        IEnumerable<Guid> ids,
        Func<List<Guid>, CancellationToken, Task<int>> count,
        string entityName,
        CancellationToken cancellationToken)
    {
        var distinct = ids.Where(id => id != Guid.Empty).Distinct().ToList();
        if (distinct.Count == 0)
            return;

        var found = await count(distinct, cancellationToken);
        if (found != distinct.Count)
            throw new ArgumentException($"{entityName} does not exist or is inactive.");
    }
}
