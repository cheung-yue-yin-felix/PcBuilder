using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class GraphicsCard : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set;  } = string.Empty;
    
    public Guid GpuId { get; set; }
    
    public int VideoMemoryGb { get; set; }
    
    public int PcieSlotsUsed { get; set; }
    
    public PcieGeneration PcieGeneration { get; set; }
    
    public decimal LengthMm { get; set; }
    
    public decimal WidthMm { get; set; }
    
    public decimal HeightMm { get; set; }
    
    public decimal PowerConsumptionWatts { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Gpu Gpu { get; set; } = null!;
    
    public virtual ICollection<GraphicsCardPowerConnector> PowerConnectors { get; set; } = new List<GraphicsCardPowerConnector>();
}