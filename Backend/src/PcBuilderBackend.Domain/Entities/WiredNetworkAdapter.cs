using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WiredNetworkAdapter : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public WiredHostInterface HostInterface { get; set; }
    
    public int MaxSpeedMbps { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}