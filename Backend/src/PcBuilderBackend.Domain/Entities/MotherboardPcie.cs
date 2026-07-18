using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardPcie : BaseEntity
{
    public Guid MotherboardId { get; set; }
    
    public PcieSlotType SlotType { get; set; }
    
    public PcieSlotLane SlotLanes { get; set; }
    
    public PcieGeneration Generation { get; set; }
    
    public int SlotCount { get; set; }
}