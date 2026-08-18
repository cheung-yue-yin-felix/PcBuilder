using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Cpu : ProductEntity
{
    public Guid SocketId { get; set; }
    public int MaxMemoryGb { get; set; }
    public Guid SeriesId { get; set; }
    public bool IntegratedGraphics { get; set; }
    public bool IncludedStockCooler { get; set; }
    public int ThermalDesignPower { get; set; }
    public int PowerConsumptionWatts { get; set; }
    public Socket Socket { get; init; } = null!;
    public CpuSeries Series { get; init; } = null!;
    public ICollection<CpuRamCompat> RamCompats { get; init; } = new List<CpuRamCompat>();
    public ICollection<CpuSupportChipset> SupportedChipsets { get; init; } = new List<CpuSupportChipset>();

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

        RamCompats.Add(ramCompat);
    }

    public void RemoveRamCompat(CpuRamCompat ramCompat)
    {
        if (!RamCompats.Any(x =>
                x.DdrGeneration == ramCompat.DdrGeneration &&
                x.RamModuleCount == ramCompat.RamModuleCount &&
                x.RamRank == ramCompat.RamRank))
            throw new ArgumentException("CPU RAM compatibility entry does not exist.");

        RamCompats.Remove(ramCompat);
    }

    public void AddSupportedChipset(CpuSupportChipset supportChipset)
    {
        if (SupportedChipsets.Any(x => x.ChipsetId == supportChipset.ChipsetId))
            throw new ArgumentException("CPU already has a support entry for this chipset.");

        SupportedChipsets.Add(supportChipset);
    }

    public void RemoveSupportedChipset(CpuSupportChipset supportChipset)
    {
        if (SupportedChipsets.All(x => x.ChipsetId != supportChipset.ChipsetId))
            throw new ArgumentException("CPU does not have a support entry for this chipset.");

        SupportedChipsets.Remove(supportChipset);
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
