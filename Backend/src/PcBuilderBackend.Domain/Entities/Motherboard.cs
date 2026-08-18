using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Motherboard : ProductEntity
{
    public Guid SocketId { get; set; }
    public Guid ChipsetId { get; set; }
    public int RamSlots { get; set; }
    public int MaxMemoryGb { get; set; }
    public int MaxDimmSizeGb { get; set; }
    public int SataPorts { get; set; }
    public int FanConnectors { get; set; }
    public int EpsConnectors { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public DdrGeneration DdrGeneration { get; set; }
    public RamFormFactor RamFormFactor { get; set; }
    public MbFormFactor FormFactor { get; set; }
    public bool WifiEnabled { get; set; }
    public bool BluetoothEnabled { get; set; }
    public Chipset Chipset { get; set; } = null!;
    public Socket Socket { get; set; } = null!;
    public ICollection<MotherboardPcie> PcieSlots { get; set; } = new List<MotherboardPcie>();
    public ICollection<MotherboardM2> M2Slots { get; set; } = new List<MotherboardM2>();
    public ICollection<MotherboardUsb> UsbPorts { get; set; } = new List<MotherboardUsb>();

    protected Motherboard()
    {
    }

    public void AddPcieSlot(MotherboardPcie pcieSlot)
    {
        if (PcieSlots.Any(x =>
                x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes &&
                x.Generation == pcieSlot.Generation))
            throw new InvalidOperationException("PCIe slot already exists.");

        PcieSlots.Add(pcieSlot);
    }

    public void RemovePcieSlot(MotherboardPcie pcieSlot)
    {
        if (!PcieSlots.Any(x =>
                x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes &&
                x.Generation == pcieSlot.Generation))
            throw new InvalidOperationException("PCIe slot does not exist.");

        PcieSlots.Remove(pcieSlot);
    }

    public void AddM2Slot(MotherboardM2 m2Slot)
    {
        if (M2Slots.Any(x => x.Key == m2Slot.Key && x.PcieGeneration == m2Slot.PcieGeneration))
            throw new InvalidOperationException("M.2 slot already exists.");

        M2Slots.Add(m2Slot);
    }

    public void RemoveM2Slot(MotherboardM2 m2Slot)
    {
        if (M2Slots.All(x => x.Key != m2Slot.Key || x.PcieGeneration != m2Slot.PcieGeneration))
            throw new InvalidOperationException("M.2 slot does not exist.");

        M2Slots.Remove(m2Slot);
    }

    public void AddUsbPort(MotherboardUsb usbPort)
    {
        if (UsbPorts.Any(x => x.UsbType == usbPort.UsbType && x.UsbVersion == usbPort.UsbVersion))
            throw new InvalidOperationException("USB port already exists.");

        UsbPorts.Add(usbPort);
    }

    public void RemoveUsbPort(MotherboardUsb usbPort)
    {
        if (UsbPorts.All(x => x.UsbType != usbPort.UsbType || x.UsbVersion != usbPort.UsbVersion))
            throw new InvalidOperationException("USB port does not exist.");

        UsbPorts.Remove(usbPort);
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
        if (PcieSlots.All(x => x.SlotType != PcieSlotType.X16))
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots);

        var maxBoardGeneration = PcieSlots.Max(x => x.Generation);
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

        var candidates = M2Slots
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
            WirelessHostInterface.Pcie => PcieSlots.All(x => x.SlotType != adapter.PcieSlotType)
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots)
                : PartsCompatibilityResult.Compatible(),
            WirelessHostInterface.M2 => adapter is { Key: M2Key.E, M2FormFactor: { } formFactor } &&
                                        M2Slots.Any(slot =>
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
            WiredHostInterface.Pcie => PcieSlots.All(x => x.SlotType != adapter.PcieSlotType)
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots)
                : PartsCompatibilityResult.Compatible(),
            _ => throw new ArgumentOutOfRangeException(nameof(adapter), "Wired host interface is not recognized.")
        };
    }

    private PartsCompatibilityResult CheckUsbCompatibility(UsbVersion usbVersion, UsbType usbType)
    {
        var sameType = UsbPorts.Where(port => port.UsbType == usbType).ToList();
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
