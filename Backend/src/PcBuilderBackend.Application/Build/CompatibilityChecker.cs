using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Build;

public interface ICompatibilityChecker
{
    Task<List<CompatibilityCheck>> CheckCompatibilityAsync(
        Guid? chassisId,
        Guid? motherboardId,
        Guid? cpuId,
        Guid? cpuCoolerId,
        Guid? ramKitId,
        Guid? graphicsCardId,
        Guid? psuId,
        List<PcBuildPartDto>? chassisFans,
        List<PcBuildPartDto>? storageDevices,
        List<PcBuildPartDto>? wiredNetworkAdapters,
        List<PcBuildPartDto>? wirelessNetworkAdapters);
}

public class CompatibilityChecker(ICatalogRepository catalog) : ICompatibilityChecker
{
    public async Task<List<CompatibilityCheck>> CheckCompatibilityAsync(
        Guid? chassisId,
        Guid? motherboardId,
        Guid? cpuId,
        Guid? cpuCoolerId,
        Guid? ramKitId,
        Guid? graphicsCardId,
        Guid? psuId,
        List<PcBuildPartDto>? chassisFans,
        List<PcBuildPartDto>? storageDevices,
        List<PcBuildPartDto>? wiredNetworkAdapters,
        List<PcBuildPartDto>? wirelessNetworkAdapters)
    {
        var results = new List<CompatibilityCheck>();

        var chassis = chassisId is { } chassisValue
            ? RequireFound(await catalog.GetChassisByIdAsync(chassisValue), chassisValue)
            : null;
        var motherboard = motherboardId is { } motherboardValue
            ? RequireFound(await catalog.GetMotherboardByIdAsync(motherboardValue), motherboardValue)
            : null;
        var cpu = cpuId is { } cpuValue
            ? RequireFound(await catalog.GetCpuByIdAsync(cpuValue), cpuValue)
            : null;
        var cpuCooler = cpuCoolerId is { } coolerValue
            ? RequireFound(await catalog.GetCpuCoolerByIdAsync(coolerValue), coolerValue)
            : null;
        var ramKit = ramKitId is { } ramValue
            ? RequireFound(await catalog.GetRamByIdAsync(ramValue), ramValue)
            : null;
        var graphicsCard = graphicsCardId is { } graphicsCardValue
            ? RequireFound(await catalog.GetGraphicsCardByIdAsync(graphicsCardValue), graphicsCardValue)
            : null;
        var psu = psuId is { } psuValue
            ? RequireFound(await catalog.GetPowerSupplyByIdAsync(psuValue), psuValue)
            : null;

        var fans = Expand(
            chassisFans,
            (await catalog.GetChassisFansByIdsAsync(DistinctIds(chassisFans))).ToDictionary(x => x.Id));
        var drives = Expand(
            storageDevices,
            (await catalog.GetStorageDevicesByIdsAsync(DistinctIds(storageDevices))).ToDictionary(x => x.Id));
        var wired = Expand(
            wiredNetworkAdapters,
            (await catalog.GetWiredNetworkAdaptersByIdsAsync(DistinctIds(wiredNetworkAdapters)))
                .ToDictionary(x => x.Id));
        var wireless = Expand(
            wirelessNetworkAdapters,
            (await catalog.GetWirelessNetworkAdaptersByIdsAsync(DistinctIds(wirelessNetworkAdapters)))
                .ToDictionary(x => x.Id));

        if (chassis is not null && motherboard is not null)
            results.Add(FromBool(
                chassis.CheckMotherboardCompatibility(motherboard),
                CompatibilityReason.PartSizeExceedsLimits,
                Ref(CompatibilitySlot.Chassis, chassis.Id),
                Ref(CompatibilitySlot.Motherboard, motherboard.Id)));

        if (motherboard is not null && cpu is not null)
            results.Add(Wrap(
                motherboard.CheckCpuCompatibility(cpu),
                Ref(CompatibilitySlot.Motherboard, motherboard.Id),
                Ref(CompatibilitySlot.Cpu, cpu.Id)));

        if (motherboard is not null && ramKit is not null)
            results.Add(FromBool(
                motherboard.CheckMemoryCompatibility(ramKit),
                CompatibilityReason.NoMatchingRamConfig,
                Ref(CompatibilitySlot.Motherboard, motherboard.Id),
                Ref(CompatibilitySlot.RamKit, ramKit.Id)));

        if (cpu is not null && ramKit is not null)
            results.Add(Wrap(
                cpu.CheckMemoryCompatibility(ramKit),
                Ref(CompatibilitySlot.Cpu, cpu.Id),
                Ref(CompatibilitySlot.RamKit, ramKit.Id)));

        if (cpu is not null && !cpu.IncludedStockCooler && cpuCooler is null)
            results.Add(Wrap(
                PartsCompatibilityResult.Incompatible(CompatibilityReason.CpuCoolerRequired),
                Ref(CompatibilitySlot.Cpu, cpu.Id),
                Ref(CompatibilitySlot.CpuCooler, null)));

        if (cpu is not null && cpuCooler is not null)
            results.Add(Wrap(
                cpuCooler.CheckCompatibility(cpu),
                Ref(CompatibilitySlot.CpuCooler, cpuCooler.Id),
                Ref(CompatibilitySlot.Cpu, cpu.Id)));

        if (chassis is not null && cpuCooler is not null)
            results.Add(FromBool(
                chassis.CheckCpuCoolerCompatibility(cpuCooler),
                CompatibilityReason.PartSizeExceedsLimits,
                Ref(CompatibilitySlot.Chassis, chassis.Id),
                Ref(CompatibilitySlot.CpuCooler, cpuCooler.Id)));

        if (ramKit is not null && cpuCooler is not null)
            results.Add(Wrap(
                cpuCooler.CheckCompatibility(ramKit),
                Ref(CompatibilitySlot.CpuCooler, cpuCooler.Id),
                Ref(CompatibilitySlot.RamKit, ramKit.Id)));

        if (chassis is not null && graphicsCard is not null)
            results.Add(FromBool(
                chassis.CheckGraphicsCardCompatibility(graphicsCard),
                CompatibilityReason.PartSizeExceedsLimits,
                Ref(CompatibilitySlot.Chassis, chassis.Id),
                Ref(CompatibilitySlot.GraphicsCard, graphicsCard.Id)));

        if (motherboard is not null && graphicsCard is not null)
            results.Add(Wrap(
                motherboard.CheckGraphicsCardCompatibility(graphicsCard),
                Ref(CompatibilitySlot.Motherboard, motherboard.Id),
                Ref(CompatibilitySlot.GraphicsCard, graphicsCard.Id)));

        if (cpu is not null && graphicsCard is null && !cpu.IntegratedGraphics)
            results.Add(Wrap(
                PartsCompatibilityResult.Incompatible(CompatibilityReason.RequiresIntegratedGraphics),
                Ref(CompatibilitySlot.Cpu, cpu.Id),
                Ref(CompatibilitySlot.GraphicsCard, null)));

        if (chassis is not null && psu is not null)
            results.Add(FromBool(
                chassis.CheckPsuCompatibility(psu),
                CompatibilityReason.PartSizeExceedsLimits,
                Ref(CompatibilitySlot.Chassis, chassis.Id),
                Ref(CompatibilitySlot.Psu, psu.Id)));

        if (psu is not null && motherboard is not null)
            results.Add(Wrap(
                psu.CheckMotherboardCompatibility(motherboard),
                Ref(CompatibilitySlot.Psu, psu.Id),
                Ref(CompatibilitySlot.Motherboard, motherboard.Id)));

        if (psu is not null && graphicsCard is not null)
            results.Add(Wrap(
                psu.CheckGraphicsCardCompatibility(graphicsCard),
                Ref(CompatibilitySlot.Psu, psu.Id),
                Ref(CompatibilitySlot.GraphicsCard, graphicsCard.Id)));

        if (psu is not null && cpu is not null)
        {
            var powerParts = new List<CompatibilityPartRef>
            {
                Ref(CompatibilitySlot.Psu, psu.Id),
                Ref(CompatibilitySlot.Cpu, cpu.Id)
            };
            if (graphicsCard is not null)
                powerParts.Add(Ref(CompatibilitySlot.GraphicsCard, graphicsCard.Id));

            results.Add(Wrap(psu.CheckPowerBudget(cpu, graphicsCard), powerParts));
        }

        if (chassis is not null && fans.Count > 0)
            results.Add(FromBool(
                chassis.CheckFanCompatibility(fans),
                CompatibilityReason.PartSizeExceedsLimits,
                Collection(
                    Ref(CompatibilitySlot.Chassis, chassis.Id),
                    CompatibilitySlot.ChassisFans,
                    fans.Select(fan => fan.Id))));

        if (chassis is not null && drives.Count > 0)
            results.Add(FromBool(
                chassis.CheckStorageDriveCompatibility(drives),
                CompatibilityReason.PartSizeExceedsLimits,
                Collection(
                    Ref(CompatibilitySlot.Chassis, chassis.Id),
                    CompatibilitySlot.StorageDevices,
                    drives.Select(drive => drive.Id))));

        if (motherboard is not null && drives.Count > 0)
            results.Add(Wrap(
                motherboard.CheckStorageCompatibility(drives),
                Collection(
                    Ref(CompatibilitySlot.Motherboard, motherboard.Id),
                    CompatibilitySlot.StorageDevices,
                    drives.Select(drive => drive.Id))));

        if (psu is not null && drives.Count > 0)
            results.Add(Wrap(
                psu.CheckStorageCompatibility(drives),
                Collection(
                    Ref(CompatibilitySlot.Psu, psu.Id),
                    CompatibilitySlot.StorageDevices,
                    drives.Select(drive => drive.Id))));

        if (motherboard is not null && (wired.Count > 0 || wireless.Count > 0))
            results.Add(Wrap(
                motherboard.CheckNetworkAdapterCompatibility(wired, wireless),
                NetworkRefs(motherboard.Id, wired.Select(adapter => adapter.Id), wireless.Select(adapter => adapter.Id))));

        return results;
    }

    private static T RequireFound<T>(T? entity, Guid id) where T : class =>
        entity ?? throw new ArgumentException($"Part {id} was not found.");

    private static List<Guid> DistinctIds(IEnumerable<PcBuildPartDto>? rows) =>
        [.. (rows ?? []).Select(row => row.PartId).Distinct()];

    private static List<T> Expand<T>(
        IEnumerable<PcBuildPartDto>? rows,
        IReadOnlyDictionary<Guid, T> byId)
        where T : class
    {
        var expanded = new List<T>();
        foreach (var row in rows ?? [])
        {
            if (!byId.TryGetValue(row.PartId, out var entity))
                throw new ArgumentException($"Part {row.PartId} was not found.", nameof(rows));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(row.Quantity);

            for (var i = 0; i < row.Quantity; i++)
                expanded.Add(entity);
        }

        return expanded;
    }

    private static CompatibilityPartRef Ref(CompatibilitySlot slot, Guid? partId) => new(slot, partId);

    private static List<CompatibilityPartRef> Collection(
        CompatibilityPartRef host,
        CompatibilitySlot slot,
        IEnumerable<Guid> partIds)
    {
        var parts = new List<CompatibilityPartRef> { host };
        parts.AddRange(partIds.Distinct().Select(id => Ref(slot, id)));
        return parts;
    }

    private static List<CompatibilityPartRef> NetworkRefs(
        Guid motherboardId,
        IEnumerable<Guid> wiredIds,
        IEnumerable<Guid> wirelessIds)
    {
        var parts = new List<CompatibilityPartRef> { Ref(CompatibilitySlot.Motherboard, motherboardId) };
        parts.AddRange(wiredIds.Distinct().Select(id => Ref(CompatibilitySlot.WiredNetworkAdapters, id)));
        parts.AddRange(wirelessIds.Distinct().Select(id => Ref(CompatibilitySlot.WirelessNetworkAdapters, id)));
        return parts;
    }

    private static CompatibilityCheck Wrap(
        PartsCompatibilityResult result,
        params CompatibilityPartRef[] parts) =>
        new(result, parts);

    private static CompatibilityCheck Wrap(
        PartsCompatibilityResult result,
        IReadOnlyList<CompatibilityPartRef> parts) =>
        new(result, parts);

    private static CompatibilityCheck FromBool(
        bool compatible,
        CompatibilityReason reason,
        params CompatibilityPartRef[] parts) =>
        Wrap(
            compatible
                ? PartsCompatibilityResult.Compatible()
                : PartsCompatibilityResult.Incompatible(reason),
            parts);

    private static CompatibilityCheck FromBool(
        bool compatible,
        CompatibilityReason reason,
        IReadOnlyList<CompatibilityPartRef> parts) =>
        Wrap(
            compatible
                ? PartsCompatibilityResult.Compatible()
                : PartsCompatibilityResult.Incompatible(reason),
            parts);
}
