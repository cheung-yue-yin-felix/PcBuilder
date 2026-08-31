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
        ArgumentNullException.ThrowIfNull(storage);
        return CheckStorageCompatibility([storage]);
    }

    /// <param name="drives">
    /// Selected drives (one entry per unit; expand quantity by repeating the product).
    /// 2.5" and 3.5" SATA drives consume <see cref="SataPorts"/>. M.2 drives consume
    /// matching M.2 <see cref="MotherboardM2.SlotCount"/>.
    /// </param>
    public PartsCompatibilityResult CheckStorageCompatibility(IEnumerable<StorageDrive> drives)
    {
        ArgumentNullException.ThrowIfNull(drives);

        var list = drives as IList<StorageDrive> ?? [.. drives];
        if (list.Count == 0)
            return PartsCompatibilityResult.Compatible();

        foreach (var drive in list)
        {
            if (drive.FormFactor is StorageFormFactor.Sata25 or StorageFormFactor.Sata35 || drive.IsM2)
                continue;

            throw new ArgumentOutOfRangeException(nameof(drives), "Storage form factor is not recognized.");
        }

        var sataBayCount = list.Count(drive =>
            drive.FormFactor is StorageFormFactor.Sata25 or StorageFormFactor.Sata35);

        if (sataBayCount > SataPorts)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientSataPorts);

        return AssignM2Demands(list.Where(drive => drive.IsM2).Select(ToM2Demand));
    }

    public PartsCompatibilityResult CheckWirelessNetworkAdapterCompatibility(WirelessNetworkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        return CheckNetworkAdapterCompatibility([], [adapter]);
    }

    public PartsCompatibilityResult CheckWirelessNetworkAdapterCompatibility(
        IEnumerable<WirelessNetworkAdapter> adapters)
    {
        ArgumentNullException.ThrowIfNull(adapters);
        return CheckNetworkAdapterCompatibility([], adapters);
    }

    public PartsCompatibilityResult CheckWiredNetworkAdapterCompatibility(WiredNetworkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        return CheckNetworkAdapterCompatibility([adapter], []);
    }

    public PartsCompatibilityResult CheckWiredNetworkAdapterCompatibility(
        IEnumerable<WiredNetworkAdapter> adapters)
    {
        ArgumentNullException.ThrowIfNull(adapters);
        return CheckNetworkAdapterCompatibility(adapters, []);
    }

    /// <param name="wired">
    /// Selected wired adapters (one entry per unit; expand quantity by repeating the product).
    /// </param>
    /// <param name="wireless">
    /// Selected wireless adapters (one entry per unit; expand quantity by repeating the product).
    /// USB ports, PCIe slots, and E-key M.2 slots are consumed in aggregate across both lists.
    /// </param>
    public PartsCompatibilityResult CheckNetworkAdapterCompatibility(
        IEnumerable<WiredNetworkAdapter> wired,
        IEnumerable<WirelessNetworkAdapter> wireless)
    {
        ArgumentNullException.ThrowIfNull(wired);
        ArgumentNullException.ThrowIfNull(wireless);

        var wiredList = wired as IList<WiredNetworkAdapter> ?? [.. wired];
        var wirelessList = wireless as IList<WirelessNetworkAdapter> ?? [.. wireless];

        var pcieNeeded = wiredList
            .Where(adapter => adapter.HostInterface == WiredHostInterface.Pcie)
            .Select(adapter => adapter.PcieSlotType!.Value)
            .Concat(wirelessList
                .Where(adapter => adapter.HostInterface == WirelessHostInterface.Pcie)
                .Select(adapter => adapter.PcieSlotType!.Value))
            .GroupBy(slotType => slotType)
            .ToList();

        foreach (var group in pcieNeeded)
        {
            var available = _pcieSlots
                .Where(slot => slot.SlotType == group.Key)
                .Sum(slot => slot.SlotCount);

            if (available < group.Count())
                return PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots);
        }

        PartsCompatibilityResult? reduced = null;

        var usbNeeded = wiredList
            .Where(adapter => adapter.HostInterface == WiredHostInterface.Usb)
            .Select(adapter => (Type: adapter.UsbType!.Value, Version: adapter.UsbVersion!.Value))
            .Concat(wirelessList
                .Where(adapter => adapter.HostInterface == WirelessHostInterface.Usb)
                .Select(adapter => (Type: adapter.UsbType!.Value, Version: adapter.UsbVersion!.Value)))
            .GroupBy(usb => usb.Type)
            .ToList();

        foreach (var group in usbNeeded)
        {
            var ports = _usbPorts.Where(port => port.UsbType == group.Key).ToList();
            if (ports.Count == 0)
                return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingUsbPort);

            if (ports.Sum(port => port.PortCount) < group.Count())
                return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingUsbPort);

            var maxVersion = ports.Max(port => port.UsbVersion);
            var rated = group.Max(usb => usb.Version);
            if (rated > maxVersion)
            {
                reduced = PartsCompatibilityResult.CompatibleReduced(
                    CompatibilityReason.UsbVersionReduced,
                    rated,
                    maxVersion);
            }
        }

        var m2Result = AssignM2Demands(
            wirelessList
                .Where(adapter => adapter.HostInterface == WirelessHostInterface.M2)
                .Select(ToM2Demand));

        if (m2Result.Status == PartsCompatibility.Incompatible)
            return m2Result;

        return reduced ?? m2Result;
    }

    private PartsCompatibilityResult AssignM2Demands(IEnumerable<M2Demand> demands)
    {
        var list = demands as IList<M2Demand> ?? [.. demands];
        if (list.Count == 0)
            return PartsCompatibilityResult.Compatible();

        var remaining = _m2Slots.ToDictionary(slot => slot, slot => slot.SlotCount);
        PartsCompatibilityResult? reduced = null;

        foreach (var demand in list.OrderBy(CountM2Candidates))
        {
            var result = TryConsumeM2Slot(demand, remaining);
            if (result.Status == PartsCompatibility.Incompatible)
                return result;

            if (result.Status == PartsCompatibility.CompatibleReduced)
                reduced = result;
        }

        return reduced ?? PartsCompatibilityResult.Compatible();
    }

    private int CountM2Candidates(M2Demand demand) =>
        _m2Slots.Count(slot => IsM2Candidate(slot, demand));

    private PartsCompatibilityResult TryConsumeM2Slot(M2Demand demand, Dictionary<MotherboardM2, int> remaining)
    {
        var candidates = _m2Slots.Where(slot => IsM2Candidate(slot, demand)).ToList();
        if (candidates.Count == 0)
        {
            if (demand.RequiresSata &&
                _m2Slots.Any(slot => MatchesM2KeyAndForm(slot, demand)))
                return PartsCompatibilityResult.Incompatible(CompatibilityReason.SlotDoesNotSupportSata);

            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingM2Slot);
        }

        var available = candidates.Where(slot => remaining[slot] > 0).ToList();
        if (available.Count == 0)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingM2Slot);

        var chosen = available.MaxBy(slot => slot.PcieGeneration)!;
        remaining[chosen]--;

        if (demand.Generation is { } generation && generation > chosen.PcieGeneration)
        {
            return PartsCompatibilityResult.CompatibleReduced(
                CompatibilityReason.PcieGenerationReduced,
                rated: generation,
                executing: chosen.PcieGeneration);
        }

        return PartsCompatibilityResult.Compatible();
    }

    private static bool IsM2Candidate(MotherboardM2 slot, M2Demand demand) =>
        MatchesM2KeyAndForm(slot, demand) && (!demand.RequiresSata || slot.SupportsSata);

    private static bool MatchesM2KeyAndForm(MotherboardM2 slot, M2Demand demand) =>
        demand.Key.FitsSlot(slot.Key) &&
        slot.FormFactors.Any(formFactor => formFactor.FormFactor == demand.FormFactor);

    private static M2Demand ToM2Demand(StorageDrive drive) =>
        new(
            drive.ModuleKey!.Value,
            drive.M2FormFactor!.Value,
            drive.Interface == StorageInterface.Sata,
            drive.Interface == StorageInterface.Nvme ? drive.PcieGeneration : null);

    private static M2Demand ToM2Demand(WirelessNetworkAdapter adapter) =>
        new(adapter.Key!.Value, adapter.M2FormFactor!.Value, RequiresSata: false, Generation: null);

    private readonly record struct M2Demand(
        M2Key Key,
        M2FormFactor FormFactor,
        bool RequiresSata,
        PcieGeneration? Generation);

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
