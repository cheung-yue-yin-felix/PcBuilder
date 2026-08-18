using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Psu : ProductEntity
{
    public int Wattage { get; set; }
    public PsuModularity Modularity { get; set; }
    public PsuFormFactor FormFactor { get; set; }
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public ICollection<PsuCable> Cables { get; set; } = new List<PsuCable>();

    private const decimal CpuVrmEfficiency = 0.90m;
    private const decimal CpuHeadroom = 1.25m;
    private const decimal GpuHeadroom = 1.30m;
    private const decimal PlatformOverheadWatts = 50m;
    
    protected Psu() {}

    public Psu(string name, Guid manufacturerId, int wattage, PsuModularity modularity, decimal lengthMm,
        decimal widthMm, decimal heightMm)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(wattage, modularity, lengthMm, widthMm, heightMm);
    }

    public void UpdateSpecs(int wattage, PsuModularity modularity, decimal lengthMm, decimal widthMm, decimal heightMm)
    {
        SetSpecs(wattage, modularity, lengthMm, widthMm, heightMm);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddCable(PsuCable cable)
    {
        if (Cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable is already added");
            
        Cables.Add(cable);
    }

    public void RemoveCable(PsuCable cable)
    {
        if (!Cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable does not exist");
        
        Cables.Remove(cable);
    }
    
    public PartsCompatibilityResult CheckPowerBudget(Cpu cpu, GraphicsCard? gpu)
    {
        var cpu12V = cpu.PowerConsumptionWatts / CpuVrmEfficiency * CpuHeadroom;
        var gpu12V = (gpu?.PowerConsumptionWatts ?? 0) * GpuHeadroom;
        var required = cpu12V + gpu12V + PlatformOverheadWatts;

        return required > Wattage
            ? PartsCompatibilityResult.Incompatible(CompatibilityReason.ExceedsPowerBudget)
            : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckMotherboardCompatibility(Motherboard motherboard)
    {
        var atx24PinCables = Cables
            .Where(x => x.Type == PsuCableType.Motherboard24Pin)
            .Sum(x => x.CablesCount);
        
        if (atx24PinCables < 1)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.MissingMotherboardPowerCable);
        
        var cpuCables = Cables
            .Where(x => x.Type == PsuCableType.Cpu4Plus4Pin)
            .Sum(x => x.CablesCount);

        return cpuCables < motherboard.EpsConnectors ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientCpuPowerCables) : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckGraphisCardCompatibility(GraphicsCard gpu)
    {
        var pcie8PinCables = Cables
            .Where(x => x.Type == PsuCableType.Pcie6Plus2Pin)
            .Sum(x => x.CablesCount);
        
        var pcie12V2X6Cables = Cables
            .Where(x => x.Type == PsuCableType.Pcie12V2X6)
            .Sum(x => x.CablesCount);
        
        var pcie12VHighPowerCables = Cables
            .Where(x => x.Type == PsuCableType.Pcie12VHighPower)
            .Sum(x => x.CablesCount);

        return gpu.PowerConnectorType switch
        {
            PsuCableType.Pcie6Plus2Pin => pcie8PinCables < gpu.PowerConnectorCount
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientPciePowerCables)
                : PartsCompatibilityResult.Compatible(),
            PsuCableType.Pcie12V2X6 => pcie12V2X6Cables < gpu.PowerConnectorCount
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientPciePowerCables)
                : PartsCompatibilityResult.Compatible(),
            PsuCableType.Pcie12VHighPower => pcie12VHighPowerCables < gpu.PowerConnectorCount
                ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientPciePowerCables)
                : PartsCompatibilityResult.Compatible(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public PartsCompatibilityResult CheckStorageCompatibility(StorageDrive storageDrive)
    {
        return CheckStorageCompatibility([storageDrive]);
    }

    public PartsCompatibilityResult CheckStorageCompatibility(List<StorageDrive> storageDrives)
    {
        if (storageDrives.All(x => x.Interface != StorageInterface.Sata))
            return PartsCompatibilityResult.Compatible();

        var sataCables = Cables
            .Where(x => x.Type == PsuCableType.Sata)
            .Sum(x => x.CablesCount);

        return sataCables < storageDrives.Count(x => x.Interface == StorageInterface.Sata)
            ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientSataCables)
            : PartsCompatibilityResult.Compatible();
    }

    private void SetSpecs(int wattage, PsuModularity modularity, decimal lengthMm,
        decimal widthMm, decimal heightMm)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wattage);
        
        if (!Enum.IsDefined(modularity))
            throw new ArgumentException("Modularity is invalid  ");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightMm);
        
        Wattage = wattage;
        Modularity = modularity;
        LengthMm = lengthMm;
        WidthMm = widthMm;
        HeightMm = heightMm;
    }
}
