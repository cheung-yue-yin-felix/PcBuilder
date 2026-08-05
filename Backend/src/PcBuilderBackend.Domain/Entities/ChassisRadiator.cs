using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisRadiator : BaseEntity
{
    public Guid ChassisId { get; set; }
    public RadiatorLength Length { get; set; }
    public RadiatorMountLocation MountLocation { get; set; }
    public int RadiatorCount { get; set; }
    public Chassis Chassis { get; set; } = null!;
    
    protected ChassisRadiator() {}

    public ChassisRadiator(Guid chassisId, RadiatorLength length, RadiatorMountLocation mountLocation)
    {
        SetSpecs(chassisId, length, mountLocation);
    }

    public void UpdateSpecs(Guid chassisId, RadiatorLength length, RadiatorMountLocation mountLocation)
    {
        SetSpecs(chassisId, length, mountLocation);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisId, RadiatorLength length, RadiatorMountLocation mountLocation)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException($"Chassis ID is required.");
        
        if (!Enum.IsDefined(length))
            throw new ArgumentException($"Radiator length is invalid.");
        
        if (!Enum.IsDefined(mountLocation))
            throw new ArgumentException($"Radiator mount location is invalid.");
        
        ChassisId = chassisId;
        Length = length;
        MountLocation = mountLocation;
    }
}
