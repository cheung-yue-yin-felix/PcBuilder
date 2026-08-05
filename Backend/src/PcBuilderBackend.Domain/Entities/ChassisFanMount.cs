using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMount : BaseEntity
{
    public Guid ChassisId { get; set; }
    public FanMountLocation Location { get; set; }
    public bool SingleDiameterOnly { get; set; }
    public Chassis Chassis { get; set; } = null!;
    public ICollection<ChassisFanMountOption> Options { get; set; } = new List<ChassisFanMountOption>();
    
    protected ChassisFanMount() { }

    public ChassisFanMount(Guid chassisId, FanMountLocation location, bool singleDiameterOnly)
    {
        SetSpecs(chassisId, location, singleDiameterOnly);
    }

    public void UpdateSpecs(Guid chassisId, FanMountLocation location, bool singleDiameterOnly)
    {
        SetSpecs(chassisId, location, singleDiameterOnly);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddOption(ChassisFanMountOption option)
    {
        if (Options.Any(x => x.Diameter == option.Diameter))
            throw new InvalidOperationException("Option already exists");
        
        Options.Add(option);
    }

    public void RemoveOption(ChassisFanMountOption option)
    {
        if (!Options.Any(x => x.Diameter == option.Diameter))
            throw new InvalidOperationException("Option does not exist");
        
        Options.Remove(option);
    }
    
    private void SetSpecs(Guid chassisId, FanMountLocation location, bool singleDiameterOnly)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("ChassisId must be a non-empty guid");
        
        if (!Enum.IsDefined(location))
            throw new ArgumentException("Invalid location");
        
        ChassisId = chassisId;
        Location = location;
        SingleDiameterOnly = singleDiameterOnly;
    }
}
