using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMountOption : BaseEntity
{
    public Guid ChassisFanMountId { get; set; }
    
    public FanDiameterMm Diameter { get; set; }
    
    public int SlotCount { get; set; }
}