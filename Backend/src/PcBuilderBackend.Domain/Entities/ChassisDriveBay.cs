using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisDriveBay : BaseEntity
{
    public Guid ChassisId { get; set; }
    public DriveBayFormFactor DriveBayFormFactor { get; set; }
    public int BayCount { get; set; }
}