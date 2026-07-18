using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMount : BaseEntity
{
    public Guid ChassisId { get; set; }
    
    public FanMountLocation Location { get; set; }
    
    public bool SingleDiameterOnly { get; set; }

    public virtual ICollection<ChassisFanMountOption> Options { get; set; } = new List<ChassisFanMountOption>();
}
