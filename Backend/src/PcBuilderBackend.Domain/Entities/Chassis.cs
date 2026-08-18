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
        decimal motherboardMaxWidthMm,
        decimal motherboardMaxHeightMm,
        decimal maxCpuCoolerHeightMm,
        decimal maxGraphicsCardLengthMm,
        decimal maxPsuLengthMm)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(
            lengthMm,
            widthMm,
            heightMm,
            motherboardMaxWidthMm,
            motherboardMaxHeightMm,
            maxCpuCoolerHeightMm,
            maxGraphicsCardLengthMm,
            maxPsuLengthMm);
    }

    public void UpdateSpecs(
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        decimal motherboardMaxWidthMm,
        decimal motherboardMaxHeightMm,
        decimal maxCpuCoolerHeightMm,
        decimal maxGraphicsCardLengthMm,
        decimal maxPsuLengthMm)
    {
        SetSpecs(
            lengthMm,
            widthMm,
            heightMm,
            motherboardMaxWidthMm,
            motherboardMaxHeightMm,
            maxCpuCoolerHeightMm,
            maxGraphicsCardLengthMm,
            maxPsuLengthMm);

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

    public bool CheckCpuCoolerCompatibility(CpuCooler cpuCooler)
    {
        return cpuCooler.Type switch
        {
            CpuCoolerType.Air => cpuCooler.CoolerHeightMm <= MaxCpuCoolerHeightMm,
            CpuCoolerType.Water => Radiators.Any(x => x.Length == cpuCooler.RadiatorLength),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <param name="fans">
    /// Selected packs (one entry per pack unit; expand quantity by repeating the product).
    /// Packs may mix diameters and products; capacity is evaluated in aggregate.
    /// Each mount location contributes at most one diameter option (e.g. 3x120 or 2x140, not both).
    /// </param>
    public bool CheckFanCompatibility(IEnumerable<ChassisFan> fans)
    {
        ArgumentNullException.ThrowIfNull(fans);

        var remaining = fans
            .GroupBy(fan => fan.DiameterMm)
            .ToDictionary(group => group.Key, group => group.Sum(fan => fan.FansCountPerPack));

        if (remaining.Count == 0)
            return true;

        return CanAssignFanMounts(FanMounts.ToList(), 0, remaining);
    }

    public bool CheckFanCompatibility(ChassisFan fan)
    {
        ArgumentNullException.ThrowIfNull(fan);
        return CheckFanCompatibility([fan]);
    }

    /// <param name="drives">
    /// Selected drives (one entry per unit; expand quantity by repeating the product).
    /// M.2 drives are ignored (motherboard-mounted). 2.5" and 3.5" drives are checked against drive bays.
    /// </param>
    public bool CheckStorageDriveCompatibility(IEnumerable<StorageDrive> drives)
    {
        ArgumentNullException.ThrowIfNull(drives);

        var requiredByBay = drives
            .Select(drive => TryMapToDriveBay(drive.FormFactor))
            .Where(bay => bay.HasValue)
            .GroupBy(bay => bay!.Value)
            .ToDictionary(group => group.Key, group => group.Count());

        if (requiredByBay.Count == 0)
            return true;

        foreach (var (bayFormFactor, needed) in requiredByBay)
        {
            var available = DriveBays
                .Where(bay => bay.DriveBayFormFactor == bayFormFactor)
                .Sum(bay => bay.BayCount);

            if (needed > available)
                return false;
        }

        return true;
    }

    public bool CheckStorageDriveCompatibility(StorageDrive drive)
    {
        ArgumentNullException.ThrowIfNull(drive);
        return CheckStorageDriveCompatibility([drive]);
    }

    private static DriveBayFormFactor? TryMapToDriveBay(StorageFormFactor formFactor) => formFactor switch
    {
        StorageFormFactor.Sata25 => DriveBayFormFactor.Inch25,
        StorageFormFactor.Sata35 => DriveBayFormFactor.Inch35,
        _ => null
    };

    private static bool CanAssignFanMounts(
        IReadOnlyList<ChassisFanMount> mounts,
        int mountIndex,
        Dictionary<FanDiameterMm, int> remaining)
    {
        if (remaining.Values.All(needed => needed <= 0))
            return true;

        if (mountIndex >= mounts.Count)
            return false;

        // Leave this mount unused.
        if (CanAssignFanMounts(mounts, mountIndex + 1, remaining))
            return true;

        foreach (var option in mounts[mountIndex].Options)
        {
            if (!remaining.TryGetValue(option.Diameter, out var needed) || needed <= 0)
                continue;

            var nextRemaining = new Dictionary<FanDiameterMm, int>(remaining)
            {
                [option.Diameter] = Math.Max(0, needed - option.SlotCount)
            };

            if (CanAssignFanMounts(mounts, mountIndex + 1, nextRemaining))
                return true;
        }

        return false;
    }

    private void SetSpecs(
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        decimal motherboardMaxWidthMm,
        decimal motherboardMaxHeightMm,
        decimal maxCpuCoolerHeightMm,
        decimal maxGraphicsCardLengthMm,
        decimal maxPsuLengthMm)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(motherboardMaxWidthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(motherboardMaxHeightMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCpuCoolerHeightMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxGraphicsCardLengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxPsuLengthMm);

        LengthMm = lengthMm;
        WidthMm = widthMm;
        HeightMm = heightMm;
        MotherboardMaxWidthMm = motherboardMaxWidthMm;
        MotherboardMaxHeightMm = motherboardMaxHeightMm;
        MaxCpuCoolerHeightMm = maxCpuCoolerHeightMm;
        MaxGraphicsCardLengthMm = maxGraphicsCardLengthMm;
        MaxPsuLengthMm = maxPsuLengthMm;
    }
}
