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
    public Socket Socket { get; init; } = null!;
    public CpuSeries Series { get; init; } = null!;
    public ICollection<CpuRamCompat> RamCompats { get; init; } = new List<CpuRamCompat>();
    
    protected Cpu() {}

    public Cpu(
        string name,
        Guid manufacturerId,
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower
    )
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(socketId, seriesId, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower);
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

    public PartsCompatibilityResult CheckMemoryCompatibility(Ram memory)
    {
        var compat = RamCompats.FirstOrDefault(x =>
            x.DdrGeneration == memory.DdrGeneration &&
            x.RamModuleCount == memory.ModulesCount &&
            x.RamRank == memory.RamRank);

        if (compat is null)
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.NoMatchingRamConfig, "No matching RAM configuration found for this CPU.");

        return compat.MaxSpeedMts < memory.MaxMemorySpeedMts ? 
            PartsCompatibilityResult.CompatibleReduced(CompatibilityReason.MemorySpeedExceedsCpuSupport, $"RAM would be running at {memory.MaxMemorySpeedMts} MT/s instead of {compat.MaxSpeedMts}") :
            PartsCompatibilityResult.Compatible();
    }

    public void UpdateSpecs(
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower)
    {
        SetSpecs(socketId, seriesId, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSpecs(
        Guid socketId,
        Guid seriesId,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower)
    {
        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID is required", nameof(socketId));
        
        if (seriesId == Guid.Empty)
            throw new ArgumentException("Series ID is required", nameof(seriesId));
        
        if (maxMemoryGb <= 0)
            throw new ArgumentException("Max Memory GB is required", nameof(maxMemoryGb));
        
        if (thermalDesignPower <= 0)
            throw new ArgumentException("Thermal Design Power is required", nameof(thermalDesignPower));
        
        SocketId = socketId;
        SeriesId = seriesId;
        MaxMemoryGb = maxMemoryGb;
        IntegratedGraphics = integratedGraphics;
        IncludedStockCooler = includedStockCooler;
        ThermalDesignPower = thermalDesignPower;
    }
}
