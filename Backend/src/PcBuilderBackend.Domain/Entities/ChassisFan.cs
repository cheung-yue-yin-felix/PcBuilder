using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFan : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public FanDiameterMm DiameterMm { get; set; }
    
    public int FansCountPerPack { get; set; }
    
    public virtual Manufacturer Manufacturer { get; set; } = null!;
}