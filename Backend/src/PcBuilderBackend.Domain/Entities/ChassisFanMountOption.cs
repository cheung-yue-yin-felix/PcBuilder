using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMountOption : BaseEntity
{
    public Guid ChassisFanMountId { get; private set; }
    public FanDiameterMm Diameter { get; private set; }
    public int SlotCount { get; private set; }
    public ChassisFanMount Mount { get; private set; } = null!;
    
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
