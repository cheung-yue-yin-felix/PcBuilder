using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisMbFormFactor: BaseEntity
{
    public Guid ChassisId { get; private set; }
    public MbFormFactor MbFormFactor { get; private set; }
    public Chassis Chassis { get; private set; } = null!; 
    
    protected ChassisMbFormFactor() {}

    public ChassisMbFormFactor(Guid chassisId, MbFormFactor mbFormFactor)
    {
        SetSpecs(chassisId, mbFormFactor);
    }

    public void UpdateSpecs(Guid chassisId, MbFormFactor mbFormFactor)
    {
        SetSpecs(chassisId, mbFormFactor);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid chassisId, MbFormFactor mbFormFactor)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("Chassis ID cannot be empty");
        
        if (!Enum.IsDefined(typeof(MbFormFactor), mbFormFactor))
            throw new ArgumentException("Motherboard form factor is invalid");
        
        ChassisId = chassisId;
        MbFormFactor = mbFormFactor;
    }
}
