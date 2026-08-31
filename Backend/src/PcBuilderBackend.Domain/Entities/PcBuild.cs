using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class PcBuild : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid ChassisId { get; private set; }
    public Guid MotherboardId { get; private set; }
    public Guid CpuId { get; private set; }
    public Guid? CpuCoolerId { get; private set; }
    public Guid RamKitId { get; private set; }
    public Guid? GraphicsCardId { get; private set; }
    public Guid PsuId { get; private set; }

    public PcBuildUser? User { get; private set; }

    private readonly List<PcBuildPart> _parts = [];

    public IReadOnlyCollection<PcBuildPart> Parts => _parts;

    public IReadOnlyCollection<PcBuildPart> StorageDevices =>
        _parts.FindAll(part => part.Type == PcBuildPartType.StorageDrive);

    public IReadOnlyCollection<PcBuildPart> ChassisFans =>
        _parts.FindAll(part => part.Type == PcBuildPartType.ChassisFan);

    public IReadOnlyCollection<PcBuildPart> WiredNetworkAdapters =>
        _parts.FindAll(part => part.Type == PcBuildPartType.WiredNetworkAdapter);

    public IReadOnlyCollection<PcBuildPart> WirelessNetworkAdapters =>
        _parts.FindAll(part => part.Type == PcBuildPartType.WirelessNetworkAdapter);

    protected PcBuild()
    {
    }

    public PcBuild(
        string name,
        string? description,
        Guid chassisId,
        Guid motherboardId,
        Guid cpuId,
        Guid? cpuCoolerId,
        Guid ramKitId,
        Guid? graphicsCardId,
        Guid psuId)
    {
        SetName(name);
        SetDescription(description);
        SetComponents(chassisId, motherboardId, cpuId, cpuCoolerId, ramKitId, graphicsCardId, psuId);
    }

    public void Update(
        string name,
        string? description,
        Guid chassisId,
        Guid motherboardId,
        Guid cpuId,
        Guid? cpuCoolerId,
        Guid ramKitId,
        Guid? graphicsCardId,
        Guid psuId)
    {
        SetName(name);
        SetDescription(description);
        SetComponents(chassisId, motherboardId, cpuId, cpuCoolerId, ramKitId, graphicsCardId, psuId);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddStorageDevice(Guid storageDeviceId, int quantity = 1)
        => AddPart(PcBuildPartType.StorageDrive, storageDeviceId, quantity, nameof(storageDeviceId));

    public void RemoveStorageDevice(Guid storageDeviceId, int quantity = 1)
        => RemovePart(PcBuildPartType.StorageDrive, storageDeviceId, quantity, nameof(storageDeviceId));

    public void AddChassisFan(Guid chassisFanId, int quantity = 1)
        => AddPart(PcBuildPartType.ChassisFan, chassisFanId, quantity, nameof(chassisFanId));

    public void RemoveChassisFan(Guid chassisFanId, int quantity = 1)
        => RemovePart(PcBuildPartType.ChassisFan, chassisFanId, quantity, nameof(chassisFanId));

    public void AddWiredNetworkAdapter(Guid wiredNetworkAdapterId, int quantity = 1)
        => AddPart(PcBuildPartType.WiredNetworkAdapter, wiredNetworkAdapterId, quantity,
            nameof(wiredNetworkAdapterId));

    public void RemoveWiredNetworkAdapter(Guid wiredNetworkAdapterId, int quantity = 1)
        => RemovePart(PcBuildPartType.WiredNetworkAdapter, wiredNetworkAdapterId, quantity,
            nameof(wiredNetworkAdapterId));

    public void AddWirelessNetworkAdapter(Guid wirelessNetworkAdapterId, int quantity = 1)
        => AddPart(PcBuildPartType.WirelessNetworkAdapter, wirelessNetworkAdapterId, quantity,
            nameof(wirelessNetworkAdapterId));

    public void RemoveWirelessNetworkAdapter(Guid wirelessNetworkAdapterId, int quantity = 1)
        => RemovePart(PcBuildPartType.WirelessNetworkAdapter, wirelessNetworkAdapterId, quantity,
            nameof(wirelessNetworkAdapterId));

    private void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Build name is required.", nameof(name));

        var trimmed = name.Trim();
        if (trimmed.Length > 100)
            throw new ArgumentException("Build name must be 100 characters or fewer.", nameof(name));

        Name = trimmed;
    }

    private void SetDescription(string? description)
    {
        if (description is { Length: > 500 })
            throw new ArgumentException("Build description must be 500 characters or fewer.", nameof(description));

        Description = description?.Trim();
    }

    private void SetComponents(
        Guid chassisId,
        Guid motherboardId,
        Guid cpuId,
        Guid? cpuCoolerId,
        Guid ramKitId,
        Guid? graphicsCardId,
        Guid psuId)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("Chassis ID is required.", nameof(chassisId));

        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID is required.", nameof(motherboardId));

        if (cpuId == Guid.Empty)
            throw new ArgumentException("CPU ID is required.", nameof(cpuId));

        if (ramKitId == Guid.Empty)
            throw new ArgumentException("RAM Kit ID is required.", nameof(ramKitId));

        if (psuId == Guid.Empty)
            throw new ArgumentException("PSU ID is required.", nameof(psuId));

        ChassisId = chassisId;
        MotherboardId = motherboardId;
        CpuId = cpuId;
        RamKitId = ramKitId;
        CpuCoolerId = OptionalId(cpuCoolerId);
        GraphicsCardId = OptionalId(graphicsCardId);
        PsuId = psuId;
    }

    private static Guid? OptionalId(Guid? id)
        => id is null || id == Guid.Empty ? null : id;

    private void AddPart(PcBuildPartType type, Guid partId, int quantity, string paramName)
    {
        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", paramName);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var existing = _parts.FirstOrDefault(part => part.Type == type && part.PartId == partId);
        if (existing is null)
            _parts.Add(new PcBuildPart(Id, type, partId, quantity));
        else
            existing.Increase(quantity);
    }

    private void RemovePart(PcBuildPartType type, Guid partId, int quantity, string paramName)
    {
        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", paramName);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var existing = _parts.FirstOrDefault(part => part.Type == type && part.PartId == partId)
            ?? throw new ArgumentException("Part does not exist.", paramName);

        if (quantity > existing.Quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity exceeds what is in the build.");

        if (quantity == existing.Quantity)
            _parts.Remove(existing);
        else
            existing.Decrease(quantity);
    }
}
