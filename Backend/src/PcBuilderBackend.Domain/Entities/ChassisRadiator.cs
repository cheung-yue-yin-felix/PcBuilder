using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisRadiator : BaseEntity
{
    public Guid ChassisId { get; set; }
    
    public RadiatorLength Length { get; set; }
    
    public RadiatorMountLocation MountLocation { get; set; }
    
    public int RadiatorCount { get; set; }
}