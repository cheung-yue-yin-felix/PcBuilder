using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Cpu : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid ManufacturerId { get; set; }

    public Guid SocketId { get; set; }
    
    public DdrGeneration DdrGeneration { get; set; }

    public int MaxMemoryGb { get; set; }
    
    public string Series { get; set; } = string.Empty;
    
    public bool IntegratedGraphics { get; set; }
    
    public bool IncludedStockCooler { get; set; }
    
    public int ThermalDesignPower { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Socket Socket { get; set; } = null!;
}