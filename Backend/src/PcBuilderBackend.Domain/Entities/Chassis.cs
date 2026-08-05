using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Chassis : ProductEntity
{
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public decimal MotherboardMaxWidthMm { get; set; }
    public decimal MotherboardMaxHeightMm { get; set; }
    public decimal MaxCpuCoolerHeightMm { get; set; }
    public decimal MaxGraphicsCardLengthMm { get; set; }
    public decimal MaxPsuLengthMm { get; set; }
    public ICollection<ChassisFanMount> FanMounts { get; set; } = new List<ChassisFanMount>();
    public ICollection<ChassisDriveBay> DriveBays { get; set; } = new List<ChassisDriveBay>();
    public ICollection<ChassisPcieSlot> PcieSlots { get; set; } = new List<ChassisPcieSlot>();
    public ICollection<ChassisRadiator> Radiators { get; set; } = new List<ChassisRadiator>();
    public ICollection<ChassisPsuFormFactor> PsuFormFactors { get; set; } = new List<ChassisPsuFormFactor>();
    public ICollection<ChassisMbFormFactor> MbFormFactors { get; set; } = new List<ChassisMbFormFactor>();
    
    protected Chassis() { }

    public Chassis(
        string name,
        Guid manufacturerId,
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        decimal maxCpuCoolerHeightMm,
        decimal maxGraphicsCardLengthMm)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(lengthMm, widthMm, heightMm, maxCpuCoolerHeightMm, maxGraphicsCardLengthMm);
    }

    public void UpdateSpecs(
        decimal lengthMm, 
        decimal widthMm, 
        decimal heightMm, 
        decimal maxCpuCoolerHeightMm, 
        decimal maxGraphicsCardLengthMm)
    {
        SetSpecs(
            lengthMm,
            widthMm,
            heightMm,
            maxCpuCoolerHeightMm,
            maxGraphicsCardLengthMm);
        
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void AddDriveBay(ChassisDriveBay driveBay)
    {
        if (DriveBays.Any(x => x.DriveBayFormFactor == driveBay.DriveBayFormFactor))
            throw new InvalidOperationException("Chassis Drive Bay already exists.");
        
        DriveBays.Add(driveBay);
    }

    public void RemoveDriveBay(ChassisDriveBay driveBay)
    {
        if (DriveBays.All(x => x.DriveBayFormFactor != driveBay.DriveBayFormFactor))
            throw new InvalidOperationException("Chassis Drive Bay does not exist.");
        
        DriveBays.Remove(driveBay);
    }
    
    public void AddFanMount(ChassisFanMount fanMount)
    {
        if (FanMounts.Any(x => x.Location == fanMount.Location))
            throw new InvalidOperationException("Chassis Fan Mount already exists.");
        
        FanMounts.Add(fanMount);
    }

    public void RemoveFanMount(ChassisFanMount fanMount)
    {
        if (FanMounts.All(x => x.Location != fanMount.Location))
            throw new InvalidOperationException("Chassis Fan Mount does not exist.");
        
        FanMounts.Remove(fanMount);
    }

    public void AddMbFormFactor(ChassisMbFormFactor mbFormFactor)
    {
        if (MbFormFactors.Any(x => x.MbFormFactor == mbFormFactor.MbFormFactor))
            throw new InvalidOperationException("Chassis Motherboard Form Factor already exists.");
        
        MbFormFactors.Add(mbFormFactor);
    }

    public void RemoveMbFormFactor(ChassisMbFormFactor mbFormFactor)
    {
        if (MbFormFactors.All(x => x.MbFormFactor != mbFormFactor.MbFormFactor)) 
            throw new InvalidOperationException("Chassis Motherboard Form Factor does not exist.");
        
        MbFormFactors.Remove(mbFormFactor);
    }

    public void AddPcieSlot(ChassisPcieSlot pcieSlot)
    {
        if (PcieSlots.Any(x => x.LowProfileSlots == pcieSlot.LowProfileSlots && x.Orientation == pcieSlot.Orientation))
            throw new InvalidOperationException("Chassis PcieSlot already exists.");
        
        PcieSlots.Add(pcieSlot);
    }

    public void RemovePcieSlot(ChassisPcieSlot pcieSlot)
    {
        if (PcieSlots.All(x => x.LowProfileSlots != pcieSlot.LowProfileSlots || x.Orientation != pcieSlot.Orientation))
            throw new InvalidOperationException("Chassis PcieSlot does not exist.");
        
        PcieSlots.Remove(pcieSlot);
    }

    public void AddPsuFormFactor(ChassisPsuFormFactor psuFormFactor)
    {
        if (PsuFormFactors.Any(x => x.PsuFormFactor == psuFormFactor.PsuFormFactor))
            throw new InvalidOperationException("Chassis Psu Form Factor already exists.");
        
        PsuFormFactors.Add(psuFormFactor);
    }

    public void RemovePsuFormFactor(ChassisPsuFormFactor psuFormFactor)
    {
        if (PsuFormFactors.All(x => x.PsuFormFactor != psuFormFactor.PsuFormFactor))
            throw new InvalidOperationException("Chassis Psu Form Factor does not exist.");
        
        PsuFormFactors.Remove(psuFormFactor);
    }

    public void AddRadiator(ChassisRadiator radiator)
    {
        if (Radiators.Any(x => x.Length == radiator.Length && x.MountLocation == radiator.MountLocation))
            throw new InvalidOperationException("Chassis Radiator already exists.");
        
        Radiators.Add(radiator);
    }

    public void RemoveRadiator(ChassisRadiator radiator)
    {
        if (Radiators.All(x => x.Length != radiator.Length || x.MountLocation != radiator.MountLocation))
            throw new InvalidOperationException("Chassis Radiator does not exist.");
        
        Radiators.Remove(radiator);
    }
    
    public bool CheckGraphicsCardCompatibility(GraphicsCard graphicsCard)
    {
        var horizontalPcieSlots = PcieSlots
            .Where(x => x.Orientation == PcieOrientation.Horizontal)
            .Where(x => x.LowProfileSlots == graphicsCard.IsLowProfile)
            .Sum(x => x.SlotCount);

        var verticalPcieSlots = PcieSlots
            .Where(x => x.Orientation == PcieOrientation.Vertical)
            .Where(x => x.LowProfileSlots == graphicsCard.IsLowProfile)
            .Sum(x => x.SlotCount);

        return graphicsCard.LengthMm <= MaxGraphicsCardLengthMm &&
               graphicsCard.PcieSlotsUsed <= horizontalPcieSlots + verticalPcieSlots;
    }

    public bool CheckMotherboardCompatibility(Motherboard motherboard)
    {
        return MbFormFactors.Any(x => x.MbFormFactor == motherboard.FormFactor) &&
               motherboard.HeightMm <= MotherboardMaxHeightMm &&
               motherboard.WidthMm <= MotherboardMaxWidthMm;
    }

    public bool CheckPsuCompatibility(Psu psu)
    {
        return PsuFormFactors.Any(x => x.PsuFormFactor == psu.FormFactor) &&
               psu.LengthMm <= MaxPsuLengthMm;
    }

    private void SetSpecs(
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        decimal maxCpuCoolerHeightMm,
        decimal maxGraphicsCardLengthMm)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCpuCoolerHeightMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxGraphicsCardLengthMm);
        
        LengthMm = lengthMm;
        WidthMm = widthMm;
        HeightMm = heightMm;
        MaxCpuCoolerHeightMm = maxCpuCoolerHeightMm;
        MaxGraphicsCardLengthMm = maxGraphicsCardLengthMm;
    }
}
