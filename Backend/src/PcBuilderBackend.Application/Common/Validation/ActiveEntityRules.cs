using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Common.Validation;

public static class ActiveEntityRules
{
    public static IRuleBuilderOptions<T, Guid> MustBeActiveManufacturer<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.ManufacturerExistsAsync(id, ct),
            "Manufacturer does not exist or is inactive.");

    public static IRuleBuilderOptions<T, Guid> MustBeActiveSocket<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.SocketExistsAsync(id, ct),
            "Socket does not exist or is inactive.");

    public static IRuleBuilderOptions<T, Guid> MustBeActiveChipset<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.ChipsetExistsAsync(id, ct),
            "Chipset does not exist or is inactive.");

    public static IRuleBuilderOptions<T, Guid> MustBeActiveCpuSeries<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.CpuSeriesExistsAsync(id, ct),
            "CPU series does not exist or is inactive.");

    public static IRuleBuilderOptions<T, Guid> MustBeActiveGpuSeries<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.GpuSeriesExistsAsync(id, ct),
            "GPU series does not exist or is inactive.");

    public static IRuleBuilderOptions<T, Guid> MustBeActiveGpu<T>(
        this IRuleBuilder<T, Guid> rule, IActiveEntityLookup lookup) =>
        rule.MustExist(lookup, (l, id, ct) => l.GpuExistsAsync(id, ct),
            "GPU does not exist or is inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveManufacturers<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountManufacturersAsync(ids, ct),
            "One or more manufacturers do not exist or are inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveSockets<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountSocketsAsync(ids, ct),
            "One or more sockets do not exist or are inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveChipsets<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountChipsetsAsync(ids, ct),
            "One or more chipsets do not exist or are inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveCpuSeries<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountCpuSeriesAsync(ids, ct),
            "One or more CPU series do not exist or are inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveGpuSeries<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountGpuSeriesAsync(ids, ct),
            "One or more GPU series do not exist or are inactive.");

    public static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllBeActiveGpus<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule, IActiveEntityLookup lookup) =>
        rule.MustAllExist(lookup, (l, ids, ct) => l.CountGpusAsync(ids, ct),
            "One or more GPUs do not exist or are inactive.");

    private static IRuleBuilderOptions<T, Guid> MustExist<T>(
        this IRuleBuilder<T, Guid> rule,
        IActiveEntityLookup lookup,
        Func<IActiveEntityLookup, Guid, CancellationToken, Task<bool>> exists,
        string message) =>
        rule.MustAsync(async (id, ct) => id != Guid.Empty && await exists(lookup, id, ct))
            .WithMessage(message);

    private static IRuleBuilderOptions<T, IEnumerable<Guid>> MustAllExist<T>(
        this IRuleBuilder<T, IEnumerable<Guid>> rule,
        IActiveEntityLookup lookup,
        Func<IActiveEntityLookup, List<Guid>, CancellationToken, Task<int>> count,
        string message) =>
        rule.MustAsync(async (ids, ct) =>
            {
                if (ids is null)
                    return true;

                var distinct = ids.Where(id => id != Guid.Empty).Distinct().ToList();
                if (distinct.Count == 0)
                    return true;

                return await count(lookup, distinct, ct) == distinct.Count;
            })
            .WithMessage(message);
}
