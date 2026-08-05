using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisPsuFormFactor: BaseEntity
{
    public Guid ChassisId { get; set; }
    public PsuFormFactor PsuFormFactor { get; set; }
    public Chassis Chassis { get; set; } = null!;
    
    protected ChassisPsuFormFactor() {}

    public ChassisPsuFormFactor(Guid chassisId, PsuFormFactor formFactor)
    {
        SetSpecs(chassisId, formFactor);
    }

    public void UpdateSpecs(Guid chassisId, PsuFormFactor formFactor)
    {
        SetSpecs(chassisId, formFactor);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisId, PsuFormFactor formFactor)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("Chassis ID cannot be empty");
        
        if (!Enum.IsDefined(formFactor))
            throw new ArgumentException("PSU Form Factor is invalid");
        
        ChassisId = chassisId;
        PsuFormFactor = formFactor;
    }
}
