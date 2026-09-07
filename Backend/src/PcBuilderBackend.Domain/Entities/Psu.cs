using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Psu : ProductEntity
{
    public int Wattage { get; private set; }
    public PsuModularity Modularity { get; private set; }
    public PsuFormFactor FormFactor { get; private set; }
    public decimal LengthMm { get; private set; }
    public decimal WidthMm { get; private set; }
    public decimal HeightMm { get; private set; }

    private readonly List<PsuCable> _cables = [];
    public IReadOnlyCollection<PsuCable> Cables => _cables;

    private const decimal CpuVrmEfficiency = 0.90m;
    private const decimal CpuHeadroom = 1.25m;
    private const decimal GpuHeadroom = 1.30m;
    private const decimal PlatformOverheadWatts = 50m;
    
    protected Psu() {}

    public Psu(string name, Guid manufacturerId, PsuSpecs specs)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(specs);
    }

    public void UpdateSpecs(PsuSpecs specs)
    {
        SetSpecs(specs);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddCable(PsuCable cable)
    {
        if (_cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable is already added");
            
        _cables.Add(cable);
    }

    public void RemoveCable(PsuCable cable)
    {
        if (!_cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable does not exist");
        
        _cables.Remove(cable);
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
        var atx24PinCables = _cables
            .Where(x => x.Type == PsuCableType.Motherboard24Pin)
            .Sum(x => x.CablesCount);
        
        if (atx24PinCables < 1)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.MissingMotherboardPowerCable);
        
        var cpuCables = _cables
            .Where(x => x.Type == PsuCableType.Cpu4Plus4Pin)
            .Sum(x => x.CablesCount);

        return cpuCables < motherboard.EpsConnectors ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientCpuPowerCables) : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckGraphicsCardCompatibility(GraphicsCard gpu)
    {
        var pcie8PinCables = _cables
            .Where(x => x.Type == PsuCableType.Pcie6Plus2Pin)
            .Sum(x => x.CablesCount);
        
        var pcie12V2X6Cables = _cables
            .Where(x => x.Type == PsuCableType.Pcie12V2X6)
            .Sum(x => x.CablesCount);
        
        var pcie12VHighPowerCables = _cables
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
            _ => throw new ArgumentOutOfRangeException(nameof(gpu), gpu.PowerConnectorType, $"Unsupported power connector '{gpu.PowerConnectorType}'.")
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

        var sataCables = _cables
            .Where(x => x.Type == PsuCableType.Sata)
            .Sum(x => x.CablesCount);

        return sataCables < storageDrives.Count(x => x.Interface == StorageInterface.Sata)
            ? PartsCompatibilityResult.Incompatible(CompatibilityReason.InsufficientSataCables)
            : PartsCompatibilityResult.Compatible();
    }

    private void SetSpecs(PsuSpecs specs)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specs.Wattage);

        if (!Enum.IsDefined(specs.Modularity))
            throw new ArgumentException("Modularity is invalid  ");

        if (!Enum.IsDefined(specs.FormFactor))
            throw new ArgumentException("Form factor is invalid");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specs.LengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specs.WidthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specs.HeightMm);

        Wattage = specs.Wattage;
        Modularity = specs.Modularity;
        FormFactor = specs.FormFactor;
        LengthMm = specs.LengthMm;
        WidthMm = specs.WidthMm;
        HeightMm = specs.HeightMm;
    }
}
