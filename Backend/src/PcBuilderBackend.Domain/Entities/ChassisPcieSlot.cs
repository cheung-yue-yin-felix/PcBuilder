using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisPcieSlot : BaseEntity
{
    public Guid ChassisId { get; set; }
    
    public bool LowProfileSlots { get; set; }
    
    public int SlotCount { get; set; }
    
    public PcieOrientation Orientation { get; set; }
}