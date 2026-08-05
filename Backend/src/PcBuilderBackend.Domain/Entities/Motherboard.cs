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
    
    protected Motherboard() {}

    public void AddPcieSlot(MotherboardPcie pcieSlot)
    {
        if (PcieSlots.Any(x => x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes && x.Generation == pcieSlot.Generation))
            throw new InvalidOperationException("PCIe slot already exists.");

        PcieSlots.Add(pcieSlot);
    }

    public void RemovePcieSlot(MotherboardPcie pcieSlot)
    {
        if (!PcieSlots.Any(x => x.SlotType == pcieSlot.SlotType && x.SlotLanes == pcieSlot.SlotLanes && x.Generation == pcieSlot.Generation))
            throw new InvalidOperationException("PCIe slot does not exist.");

        PcieSlots.Remove(pcieSlot);
    }

    public void AddM2Slot(MotherboardM2 m2Slot)
    {
        if (M2Slots.Any(x => x.PcieGeneration == m2Slot.PcieGeneration))
            throw new InvalidOperationException("M.2 slot already exists.");

        M2Slots.Add(m2Slot);
    }

    public void RemoveM2Slot(MotherboardM2 m2Slot)
    {
        if (M2Slots.All(x => x.PcieGeneration != m2Slot.PcieGeneration))
            throw new InvalidOperationException("M.2 slot does not exist.");

        M2Slots.Remove(m2Slot);
    }

    public bool CheckMemoryCompatibility(Ram memory)
    {
        return memory.DdrGeneration == DdrGeneration &&
               memory.RamFormFactor == RamFormFactor &&
               memory.TotalMemorySizeGb <= MaxMemoryGb &&
               memory.MemorySizePerStickGb <= MaxDimmSizeGb &&
               memory.ModulesCount <= RamSlots;
    }

    public PartsCompatibilityResult CheckGraphicsCardCompatibility(GraphicsCard graphicsCard)
    {
        if (graphicsCard.PcieSlotsUsed > PcieSlots.Count(x => x.SlotType == PcieSlotType.X16))
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NotEnoughPcieSlots, "Not enough PCIe slots available on the motherboard.");
        
        return graphicsCard.PcieGeneration > PcieSlots.Max(x => x.Generation) ? 
            PartsCompatibilityResult.CompatibleReduced(CompatibilityReason.PcieGenerationReduced, $"Graphics card would be running {graphicsCard.PcieGeneration} instead of {PcieSlots.Max(x => x.Generation)}") :
            PartsCompatibilityResult.Compatible();
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
        SetSpecs(socketId, chipsetId, ramSlots, maxMemoryGb, maxDimmSizeGb, sataPorts, fanConnectors, epsConnectors, widthMm, heightMm, ddrGeneration, ramFormFactor, mbFormFactor, wifiEnabled, bluetoothEnabled);
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
        SetSpecs(socketId, chipsetId, ramSlots, maxMemoryGb, maxDimmSizeGb, sataPorts, fanConnectors, epsConnectors, widthMm, heightMm, ddrGeneration, ramFormFactor, mbFormFactor, wifiEnabled, bluetoothEnabled);
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
