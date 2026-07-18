using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Ram : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;
    
    public DdrGeneration DdrGeneration { get; set; }
    
    public RamFormFactor RamFormFactor { get; set; }
    
    public int MemorySizePerStickGb { get; set; }
    
    public int TotalMemorySizeGb { get; set; }
    
    public int ModulesCount { get; set; }
    
    public int MaxMemorySpeedMts { get; set; }
    
    public double HeightMm { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}