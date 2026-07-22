using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Cpu : NamedEntity
{
    public Guid ManufacturerId { get; set; }
    public Guid SocketId { get; set; }
    public DdrGeneration DdrGeneration { get; set; }
    public int MaxMemoryGb { get; set; }
    public Guid SeriesId { get; set; }
    public bool IntegratedGraphics { get; set; }
    public bool IncludedStockCooler { get; set; }
    public int ThermalDesignPower { get; set; }
    public virtual Manufacturer Manufacturer { get; init; } = null!;
    public virtual Socket Socket { get; init; } = null!;
    public virtual CpuSeries Series { get; init; } = null!;
    
    protected Cpu() {}

    public Cpu(
        string name,
        Guid manufacturerId,
        Guid socketId,
        Guid seriesId,
        DdrGeneration ddrGeneration,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower
    )
    {
        SetName(name);
        SetSpecs(manufacturerId, socketId, seriesId, ddrGeneration, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower);
    }
    
    public bool IsCompatibleWithSocket(Socket socket) => socket.Id == SocketId;
    
    public bool SupportMemory(DdrGeneration ddrGeneration, int memoryGb) => DdrGeneration == ddrGeneration && memoryGb <= MaxMemoryGb;
    
    public bool HasIntegratedGraphics() => IntegratedGraphics;
    
    public bool IncludesStockCooler() => IncludedStockCooler;
    
    public void UpdateSpecs(
        Guid manufacturerId,
        Guid socketId,
        Guid seriesId,
        DdrGeneration ddrGeneration,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower)
    {
        SetSpecs(manufacturerId, socketId, seriesId, ddrGeneration, maxMemoryGb, integratedGraphics, includedStockCooler, thermalDesignPower);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSpecs(
        Guid manufacturerId,
        Guid socketId,
        Guid seriesId,
        DdrGeneration ddrGeneration,
        int maxMemoryGb,
        bool integratedGraphics,
        bool includedStockCooler,
        int thermalDesignPower)
    {
        if (manufacturerId == Guid.Empty)
            throw new ArgumentException("Manufacturer ID is required", nameof(manufacturerId));
        
        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID is required", nameof(socketId));
        
        if (seriesId == Guid.Empty)
            throw new ArgumentException("Series ID is required", nameof(seriesId));
        
        if (!Enum.IsDefined(ddrGeneration))
            throw new ArgumentException("DDR Generation is not valid", nameof(ddrGeneration));
        
        if (maxMemoryGb <= 0)
            throw new ArgumentException("Max Memory GB is required", nameof(maxMemoryGb));
        
        if (thermalDesignPower <= 0)
            throw new ArgumentException("Thermal Design Power is required", nameof(thermalDesignPower));
        
        ManufacturerId = manufacturerId;
        SocketId = socketId;
        SeriesId = seriesId;
        DdrGeneration = ddrGeneration;
        MaxMemoryGb = maxMemoryGb;
        IntegratedGraphics = integratedGraphics;
        IncludedStockCooler = includedStockCooler;
        ThermalDesignPower = thermalDesignPower;
    }
}