using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMountOption : BaseEntity
{
    public Guid ChassisFanMountId { get; set; }
    public FanDiameterMm Diameter { get; set; }
    public int SlotCount { get; set; }
    public ChassisFanMount Mount { get; set; } = null!;
    
    protected ChassisFanMountOption() {}

    public ChassisFanMountOption(Guid chassisFanMountId, FanDiameterMm diameter, int slotCount)
    {
        SetSpecs(chassisFanMountId, diameter, slotCount);
    }

    public void UpdateSpecs(Guid chassisFanMountId, FanDiameterMm diameter, int slotCount)
    {
        SetSpecs(chassisFanMountId, diameter, slotCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisFanMountId, FanDiameterMm diameterMm, int slotCount)
    {
        if (chassisFanMountId == Guid.Empty)
            throw new ArgumentException("Chassis Fan Mount ID must be provided");
        
        if (!Enum.IsDefined(diameterMm))
            throw new ArgumentException("Fan Diameter mm must be provided");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(slotCount);
        
        ChassisFanMountId = chassisFanMountId;
        Diameter = diameterMm;
        SlotCount = slotCount;
    }
}
