using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Psu : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int Wattage { get; set; }
    
    public PsuModularity Modularity { get; set; }
    
    public PsuFormFactor FormFactor { get; set; }
    
    public decimal LengthMm { get; set; }
    
    public decimal WidthMm { get; set; }
    
    public decimal HeightMm { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
    
    public virtual ICollection<PsuCable> Cables { get; set; } = new List<PsuCable>();
}