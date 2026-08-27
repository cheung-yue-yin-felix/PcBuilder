namespace PcBuilderBackend.Domain.Entities;

public class UserBuild : BaseEntity
{
    public Guid? UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsPublic { get; private set; }
    public Guid ChassisId { get; private set; }
    public Guid MotherboardId { get; private set; }
    public Guid CpuId { get; private set; }
    public Guid? CpuCoolerId { get; private set; }
    public Guid RamKitId { get; private set; }
    public Guid? GraphicsCardId { get; private set; }

    private readonly List<UserBuildPart> _storageDevices = [];
    public IReadOnlyCollection<UserBuildPart> StorageDevices => _storageDevices;

    private readonly List<UserBuildPart> _chassisFans = [];
    public IReadOnlyCollection<UserBuildPart> ChassisFans => _chassisFans;

    private readonly List<UserBuildPart> _wiredNetworkAdapters = [];
    public IReadOnlyCollection<UserBuildPart> WiredNetworkAdapters => _wiredNetworkAdapters;

    private readonly List<UserBuildPart> _wirelessNetworkAdapters = [];
    public IReadOnlyCollection<UserBuildPart> WirelessNetworkAdapters => _wirelessNetworkAdapters;

    protected UserBuild()
    {
    }

    public UserBuild(
        Guid? userId,
        string name,
        string? description,
        bool isPublic,
        Guid chassisId,
        Guid motherboardId,
        Guid cpuId,
        Guid? cpuCoolerId,
        Guid ramKitId,
        Guid? graphicsCardId)
    {
        UserId = OptionalId(userId);
        IsPublic = isPublic;
        SetName(name);
        SetDescription(description);
        SetComponents(chassisId, motherboardId, cpuId, cpuCoolerId, ramKitId, graphicsCardId);
    }

    public void Update(
        string name,
        string? description,
        bool isPublic,
        Guid chassisId,
        Guid motherboardId,
        Guid cpuId,
        Guid? cpuCoolerId,
        Guid ramKitId,
        Guid? graphicsCardId)
    {
        IsPublic = isPublic;
        SetName(name);
        SetDescription(description);
        SetComponents(chassisId, motherboardId, cpuId, cpuCoolerId, ramKitId, graphicsCardId);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddStorageDevice(Guid storageDeviceId, int quantity = 1)
        => AddPart(_storageDevices, storageDeviceId, quantity, nameof(storageDeviceId));

    public void RemoveStorageDevice(Guid storageDeviceId, int quantity = 1)
        => RemovePart(_storageDevices, storageDeviceId, quantity, nameof(storageDeviceId));

    public void AddChassisFan(Guid chassisFanId, int quantity = 1)
        => AddPart(_chassisFans, chassisFanId, quantity, nameof(chassisFanId));

    public void RemoveChassisFan(Guid chassisFanId, int quantity = 1)
        => RemovePart(_chassisFans, chassisFanId, quantity, nameof(chassisFanId));

    public void AddWiredNetworkAdapter(Guid wiredNetworkAdapterId, int quantity = 1)
        => AddPart(_wiredNetworkAdapters, wiredNetworkAdapterId, quantity, nameof(wiredNetworkAdapterId));

    public void RemoveWiredNetworkAdapter(Guid wiredNetworkAdapterId, int quantity = 1)
        => RemovePart(_wiredNetworkAdapters, wiredNetworkAdapterId, quantity, nameof(wiredNetworkAdapterId));

    public void AddWirelessNetworkAdapter(Guid wirelessNetworkAdapterId, int quantity = 1)
        => AddPart(_wirelessNetworkAdapters, wirelessNetworkAdapterId, quantity, nameof(wirelessNetworkAdapterId));

    public void RemoveWirelessNetworkAdapter(Guid wirelessNetworkAdapterId, int quantity = 1)
        => RemovePart(_wirelessNetworkAdapters, wirelessNetworkAdapterId, quantity, nameof(wirelessNetworkAdapterId));

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
        Guid? graphicsCardId)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("Chassis ID is required.", nameof(chassisId));

        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID is required.", nameof(motherboardId));

        if (cpuId == Guid.Empty)
            throw new ArgumentException("CPU ID is required.", nameof(cpuId));

        if (ramKitId == Guid.Empty)
            throw new ArgumentException("RAM Kit ID is required.", nameof(ramKitId));

        ChassisId = chassisId;
        MotherboardId = motherboardId;
        CpuId = cpuId;
        RamKitId = ramKitId;
        CpuCoolerId = OptionalId(cpuCoolerId);
        GraphicsCardId = OptionalId(graphicsCardId);
    }

    private static Guid? OptionalId(Guid? id)
        => id is null || id == Guid.Empty ? null : id;

    private static void AddPart(ICollection<UserBuildPart> parts, Guid partId, int quantity, string paramName)
    {
        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", paramName);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var existing = parts.FirstOrDefault(p => p.PartId == partId);
        if (existing is null)
            parts.Add(new UserBuildPart(partId, quantity));
        else
            existing.Increase(quantity);
    }

    private static void RemovePart(ICollection<UserBuildPart> parts, Guid partId, int quantity, string paramName)
    {
        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", paramName);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var existing = parts.FirstOrDefault(p => p.PartId == partId)
            ?? throw new ArgumentException("Part does not exist.", paramName);

        if (quantity > existing.Quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity exceeds what is in the build.");

        if (quantity == existing.Quantity)
            parts.Remove(existing);
        else
            existing.Decrease(quantity);
    }
}
