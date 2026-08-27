using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Cpu : ProductEntity
{
    public Guid SocketId { get; private set; }
    public int MaxMemoryGb { get; private set; }
    public Guid SeriesId { get; private set; }
    public bool IntegratedGraphics { get; private set; }
    public bool IncludedStockCooler { get; private set; }
    public int ThermalDesignPower { get; private set; }
    public int PowerConsumptionWatts { get; private set; }
    public Socket Socket { get; init; } = null!;
    public CpuSeries Series { get; init; } = null!;
    
    private readonly List<CpuRamCompat> _ramCompats = [];
    public IReadOnlyCollection<CpuRamCompat> RamCompats => _ramCompats;

    private readonly List<CpuSupportChipset> _supportedChipsets = [];
    public IReadOnlyCollection<CpuSupportChipset> SupportedChipsets => _supportedChipsets;
    
    protected Cpu()
    {
    }

    public Cpu(
        string name,
        Guid manufacturerId,
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower,
        int powerConsumptionWatts
    )
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(socketId, seriesId, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower, powerConsumptionWatts);
    }

    public void AddRamCompat(CpuRamCompat ramCompat)
    {
        if (RamCompats.Any(x =>
                x.DdrGeneration == ramCompat.DdrGeneration &&
                x.RamModuleCount == ramCompat.RamModuleCount &&
                x.RamRank == ramCompat.RamRank))
            throw new ArgumentException("CPU RAM compatibility entry already exists.");

        _ramCompats.Add(ramCompat);
    }

    public void RemoveRamCompat(CpuRamCompat ramCompat)
    {
        if (!RamCompats.Any(x =>
                x.DdrGeneration == ramCompat.DdrGeneration &&
                x.RamModuleCount == ramCompat.RamModuleCount &&
                x.RamRank == ramCompat.RamRank))
            throw new ArgumentException("CPU RAM compatibility entry does not exist.");

        _ramCompats.Remove(ramCompat);
    }

    public void AddSupportedChipset(CpuSupportChipset supportChipset)
    {
        if (SupportedChipsets.Any(x => x.ChipsetId == supportChipset.ChipsetId))
            throw new ArgumentException("CPU already has a support entry for this chipset.");

        _supportedChipsets.Add(supportChipset);
    }

    public void RemoveSupportedChipset(CpuSupportChipset supportChipset)
    {
        if (SupportedChipsets.All(x => x.ChipsetId != supportChipset.ChipsetId))
            throw new ArgumentException("CPU does not have a support entry for this chipset.");

        _supportedChipsets.Remove(supportChipset);
    }

    public PartsCompatibilityResult CheckMemoryCompatibility(Ram memory)
    {
        var compat = RamCompats.FirstOrDefault(x =>
            x.DdrGeneration == memory.DdrGeneration &&
            x.RamModuleCount == memory.ModulesCount &&
            x.RamRank == memory.RamRank);

        if (compat is null)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingRamConfig);

        return compat.MaxSpeedMts < memory.MaxMemorySpeedMts
            ? PartsCompatibilityResult.CompatibleReduced(
                CompatibilityReason.MemorySpeedExceedsCpuSupport,
                rated: memory.MaxMemorySpeedMts,
                executing: compat.MaxSpeedMts)
            : PartsCompatibilityResult.Compatible();
    }


    public void UpdateSpecs(
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower,
        int powerConsumptionWatts)
    {
        SetSpecs(socketId, seriesId, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower, powerConsumptionWatts);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower,
        int powerConsumptionWatts)
    {
        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID is required", nameof(socketId));

        if (seriesId == Guid.Empty)
            throw new ArgumentException("Series ID is required", nameof(seriesId));

        if (maxMemoryGb <= 0)
            throw new ArgumentException("Max Memory GB is required", nameof(maxMemoryGb));

        if (thermalDesignPower <= 0)
            throw new ArgumentException("Thermal Design Power is required", nameof(thermalDesignPower));
        
        if (powerConsumptionWatts <= 0)
            throw new ArgumentException("Power Consumption Watts is required", nameof(powerConsumptionWatts));

        SocketId = socketId;
        SeriesId = seriesId;
        MaxMemoryGb = maxMemoryGb;
        IntegratedGraphics = integratedGraphics;
        IncludedStockCooler = includedStockCooler;
        ThermalDesignPower = thermalDesignPower;
        PowerConsumptionWatts = powerConsumptionWatts;
    }
}
