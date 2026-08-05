using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisDriveBay : BaseEntity
{
    public Guid ChassisId { get; set; }
    public DriveBayFormFactor DriveBayFormFactor { get; set; }
    public int BayCount { get; set; }
    public Chassis Chassis { get; set; } = null!;
    
    protected ChassisDriveBay() {}

    public ChassisDriveBay(Guid chassisId, DriveBayFormFactor formFactor, int bayCount)
    {
        SetSpecs(chassisId, formFactor, bayCount);
    }

    public void UpdateSpecs(Guid chassisId, DriveBayFormFactor formFactor, int bayCount)
    {
        SetSpecs(chassisId, formFactor, bayCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisId, DriveBayFormFactor formFactor, int bayCount)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("Chassis ID cannot be empty.");
        
        if (!Enum.IsDefined(formFactor))
            throw new ArgumentException("Drive bay form factor is invalid.");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bayCount);
        
        ChassisId = chassisId;
        DriveBayFormFactor = formFactor;
        BayCount = bayCount;
    }
}
