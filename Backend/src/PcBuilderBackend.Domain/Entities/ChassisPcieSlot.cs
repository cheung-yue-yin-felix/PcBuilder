using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisPcieSlot : BaseEntity
{
    public Guid ChassisId { get; private set; }
    public bool LowProfileSlots { get; private set; }
    public int SlotCount { get; private set; }
    public PcieOrientation Orientation { get; private set; }
    public Chassis Chassis { get; private set; } = null!;
    
    protected ChassisPcieSlot() {}

    public ChassisPcieSlot(Guid chassisId, bool lowProfileSlots, int slotCount, PcieOrientation orientation)
    {
        SetSpecs(chassisId, lowProfileSlots, slotCount, orientation);
    }

    public void UpdateSpecs(Guid chassisId, bool lowProfileSlots, int slotCount, PcieOrientation orientation)
    {
        SetSpecs(chassisId, lowProfileSlots, slotCount, orientation);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisId, bool lowProfileSlots, int slotCount, PcieOrientation orientation)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException($"Chassis ID cannot be empty.");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(slotCount);
        
        if (!Enum.IsDefined(orientation))
            throw new ArgumentException($"PCI-E orientation is invalid.");
        
        ChassisId = chassisId;
        LowProfileSlots = lowProfileSlots;
        SlotCount = slotCount;
        Orientation = orientation;
    }
    
}
