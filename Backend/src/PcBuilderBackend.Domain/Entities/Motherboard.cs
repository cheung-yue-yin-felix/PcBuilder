using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Motherboard : ProductEntity
{
    public Guid SocketId { get; private set; }
    public Guid ChipsetId { get; private set; }
    public int RamSlots { get; private set; }
    public int MaxMemoryGb { get; private set; }
    public int MaxDimmSizeGb { get; private set; }
    public int SataPorts { get; private set; }
    public int FanConnectors { get; private set; }
    public int EpsConnectors { get; private set; }
    public decimal WidthMm { get; private set; }
    public decimal HeightMm { get; private set; }
    public DdrGeneration DdrGeneration { get; private set; }
    public RamFormFactor RamFormFactor { get; private set; }
    public MbFormFactor FormFactor { get; private set; }
    public bool WifiEnabled { get; private set; }
    public bool BluetoothEnabled { get; private set; }
    public Chipset Chipset { get; private set; } = null!;
    public Socket Socket { get; private set; } = null!;

    private readonly List<MotherboardPcie> _pcieSlots = [];
    public IReadOnlyCollection<MotherboardPcie> PcieSlots => _pcieSlots.AsReadOnly();

    private readonly List<MotherboardM2> _m2Slots = [];
    public IReadOnlyCollection<MotherboardM2> M2Slots => _m2Slots.AsReadOnly();

    private readonly List<MotherboardUsb> _usbPorts = [];
    public IReadOnlyCollection<MotherboardUsb> UsbPorts => _usbPorts.AsReadOnly();

    protected Motherboard()
    {
    }

    public void AddPcieSlot(MotherboardPcie pcieSlot)
    {
        if (_pcieSlots.Any(x =>
                x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes &&
                x.Generation == pcieSlot.Generation))
            throw new ArgumentException("PCIe slot already exists.");

        _pcieSlots.Add(pcieSlot);
    }

    public void RemovePcieSlot(MotherboardPcie pcieSlot)
    {
        if (!_pcieSlots.Any(x =>
                x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes &&
                x.Generation == pcieSlot.Generation))
            throw new ArgumentException("PCIe slot does not exist.");

        _pcieSlots.Remove(pcieSlot);
    }

    public void AddM2Slot(MotherboardM2 m2Slot)
    {
        if (_m2Slots.Any(x => x.Key == m2Slot.Key && x.PcieGeneration == m2Slot.PcieGeneration))
            throw new ArgumentException("M.2 slot already exists.");

        _m2Slots.Add(m2Slot);
    }

    public void RemoveM2Slot(MotherboardM2 m2Slot)
    {
        if (_m2Slots.All(x => x.Key != m2Slot.Key || x.PcieGeneration != m2Slot.PcieGeneration))
            throw new ArgumentException("M.2 slot does not exist.");

        _m2Slots.Remove(m2Slot);
    }

    public void AddUsbPort(MotherboardUsb usbPort)
    {
        if (_usbPorts.Any(x => x.UsbType == usbPort.UsbType && x.UsbVersion == usbPort.UsbVersion))
            throw new ArgumentException("USB port already exists.");

        _usbPorts.Add(usbPort);
    }

    public void RemoveUsbPort(MotherboardUsb usbPort)
    {
        if (_usbPorts.All(x => x.UsbType != usbPort.UsbType || x.UsbVersion != usbPort.UsbVersion))
            throw new ArgumentException("USB port does not exist.");

        _usbPorts.Remove(usbPort);
    }

    public bool CheckMemoryCompatibility(Ram memory)
    {
        return memory.DdrGeneration == DdrGeneration &&
               memory.RamFormFactor == RamFormFactor &&
               memory.TotalMemorySizeGb <= MaxMemoryGb &&
               memory.MemorySizePerStickGb <= MaxDimmSizeGb &&
               memory.ModulesCount <= RamSlots;
    }

    public PartsCompatibilityResult CheckCpuCompatibility(Cpu cpu)
    {
        if (cpu.SocketId != SocketId)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.SocketMismatch);

        var support = cpu.SupportedChipsets.FirstOrDefault(x => x.ChipsetId == ChipsetId);

        if (support is null)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.ChipsetNotSupported);

        if (support.RequiresBiosUpdate)
            return PartsCompatibilityResult.CompatibleActionRequired(CompatibilityReason.RequiresBiosUpdate);

        return PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckGraphicsCardCompatibility(GraphicsCard graphicsCard)
    {
        if (_pcieSlots.All(x => x.SlotType != PcieSlotType.X16))
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots);

        var maxBoardGeneration = _pcieSlots.Max(x => x.Generation);
        return graphicsCard.PcieGeneration > maxBoardGeneration
            ? PartsCompatibilityResult.CompatibleReduced(
                CompatibilityReason.PcieGenerationReduced,
                rated: graphicsCard.PcieGeneration,
                executing: maxBoardGeneration)
            : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckStorageCompatibility(StorageDrive storage)
    {
        if (storage.FormFactor is StorageFormFactor.Sata25 or StorageFormFactor.Sata35)
        {
            return SataPorts < 1
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientSataPorts)
                : PartsCompatibilityResult.Compatible();
        }

        if (storage.ModuleKey is not { } moduleKey || storage.M2FormFactor is not { } formFactor)
            throw new ArgumentOutOfRangeException(nameof(storage), "Storage form factor is not recognized.");

        var candidates = _m2Slots
            .Where(slot => moduleKey.FitsSlot(slot.Key))
            .Where(slot => slot.FormFactors.Any(x => x.FormFactor == formFactor))
            .ToList();

        if (candidates.Count == 0)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingM2Slot);

        if (storage.Interface == StorageInterface.Sata)
        {
            return candidates.Any(slot => slot.SupportsSata)
                ? PartsCompatibilityResult.Compatible()
                : PartsCompatibilityResult.Incompatible(CompatibilityReason.SlotDoesNotSupportSata);
        }

        var maxGeneration = candidates.Max(slot => slot.PcieGeneration);
        var driveGeneration = storage.PcieGeneration!.Value;
        return driveGeneration > maxGeneration
            ? PartsCompatibilityResult.CompatibleReduced(
                CompatibilityReason.PcieGenerationReduced,
                rated: driveGeneration,
                executing: maxGeneration)
            : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckWirelessNetworkAdapterCompatibility(WirelessNetworkAdapter adapter)
    {
        return adapter.HostInterface switch
        {
            WirelessHostInterface.Usb => CheckUsbCompatibility(adapter.UsbVersion!.Value, adapter.UsbType!.Value),
            WirelessHostInterface.Pcie => _pcieSlots.All(x => x.SlotType != adapter.PcieSlotType)
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots)
                : PartsCompatibilityResult.Compatible(),
            WirelessHostInterface.M2 => adapter is { Key: M2Key.E, M2FormFactor: { } formFactor } &&
                                        _m2Slots.Any(slot =>
                                            slot.Key == M2Key.E &&
                                            slot.FormFactors.Any(x => x.FormFactor == formFactor))
                ? PartsCompatibilityResult.Compatible()
                : PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingM2Slot),
            _ => throw new ArgumentOutOfRangeException(nameof(adapter), "Wireless host interface is not recognized.")
        };
    }

    public PartsCompatibilityResult CheckWiredNetworkAdapterCompatibility(WiredNetworkAdapter adapter)
    {
        return adapter.HostInterface switch
        {
            WiredHostInterface.Usb => CheckUsbCompatibility(adapter.UsbVersion!.Value, adapter.UsbType!.Value),
            WiredHostInterface.Pcie => _pcieSlots.All(x => x.SlotType != adapter.PcieSlotType)
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots)
                : PartsCompatibilityResult.Compatible(),
            _ => throw new ArgumentOutOfRangeException(nameof(adapter), "Wired host interface is not recognized.")
        };
    }

    private PartsCompatibilityResult CheckUsbCompatibility(UsbVersion usbVersion, UsbType usbType)
    {
        var sameType = _usbPorts.Where(port => port.UsbType == usbType).ToList();
        if (sameType.Count == 0)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingUsbPort);

        var maxVersion = sameType.Max(port => port.UsbVersion);
        return usbVersion > maxVersion
            ? PartsCompatibilityResult.CompatibleReduced(
                CompatibilityReason.UsbVersionReduced,
                rated: usbVersion,
                executing: maxVersion)
            : PartsCompatibilityResult.Compatible();
    }

    public Motherboard(
        Guid manufacturerId,
        string name,
        Guid socketId,
        Guid chipsetId,
        int ramSlots,
        int maxMemoryGb,
        int maxDimmSizeGb,
        int sataPorts,
        int fanConnectors,
        int epsConnectors,
        decimal widthMm,
        decimal heightMm,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        MbFormFactor mbFormFactor,
        bool wifiEnabled,
        bool bluetoothEnabled)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(
            socketId,
            chipsetId,
            ramSlots,
            maxMemoryGb,
            maxDimmSizeGb,
            sataPorts,
            fanConnectors,
            epsConnectors,
            widthMm,
            heightMm,
            ddrGeneration,
            ramFormFactor,
            mbFormFactor,
            wifiEnabled,
            bluetoothEnabled
        );
    }

    public void UpdateSpecs(
        Guid socketId,
        Guid chipsetId,
        int ramSlots,
        int maxMemoryGb,
        int maxDimmSizeGb,
        int sataPorts,
        int fanConnectors,
        int epsConnectors,
        decimal widthMm,
        decimal heightMm,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        MbFormFactor mbFormFactor,
        bool wifiEnabled,
        bool bluetoothEnabled)
    {
        SetSpecs(
            socketId,
            chipsetId,
            ramSlots,
            maxMemoryGb,
            maxDimmSizeGb,
            sataPorts,
            fanConnectors,
            epsConnectors,
            widthMm,
            heightMm,
            ddrGeneration,
            ramFormFactor,
            mbFormFactor,
            wifiEnabled,
            bluetoothEnabled);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        Guid socketId,
        Guid chipsetId,
        int ramSlots,
        int maxMemoryGb,
        int maxDimmSizeGb,
        int sataPorts,
        int fanConnectors,
        int epsConnectors,
        decimal widthMm,
        decimal heightMm,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        MbFormFactor mbFormFactor,
        bool wifiEnabled,
        bool bluetoothEnabled)
    {
        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID cannot be empty");

        if (chipsetId == Guid.Empty)
            throw new ArgumentException("Chipset ID cannot be empty");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ramSlots, "Ram Slots cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxDimmSizeGb, "Max DIMM Size GB cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxMemoryGb, "Max Memory GB cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sataPorts, "SATA Ports cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fanConnectors, "Fan Connectors cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(epsConnectors, "EPS Connectors cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthMm, "Width mm cannot be negative");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightMm, "Height mm cannot be negative");

        if (!Enum.IsDefined(ddrGeneration))
            throw new ArgumentException("DDR Generation is invalid");

        if (!Enum.IsDefined(mbFormFactor))
            throw new ArgumentException("Motherboard Form Factor is invalid");

        if (!Enum.IsDefined(ramFormFactor))
            throw new ArgumentException("RAM Form Factor is invalid");

        SocketId = socketId;
        ChipsetId = chipsetId;
        RamSlots = ramSlots;
        MaxMemoryGb = maxMemoryGb;
        MaxDimmSizeGb = maxDimmSizeGb;
        SataPorts = sataPorts;
        FanConnectors = fanConnectors;
        EpsConnectors = epsConnectors;
        DdrGeneration = ddrGeneration;
        RamFormFactor = ramFormFactor;
        FormFactor = mbFormFactor;
        WidthMm = widthMm;
        HeightMm = heightMm;
        WifiEnabled = wifiEnabled;
        BluetoothEnabled = bluetoothEnabled;
    }
}
