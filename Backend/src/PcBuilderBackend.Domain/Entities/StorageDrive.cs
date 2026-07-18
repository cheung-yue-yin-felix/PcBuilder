using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class StorageDrive : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public StorageMedia Media { get; set; }

    public StorageInterface Interface { get; set; }
    
    public StorageFormFactor FormFactor { get; set; }
    
    public int CapacityGb { get; set; }
    
    public PcieGeneration? PcieGeneration { get; set; }
    
    public int? Rpm { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}