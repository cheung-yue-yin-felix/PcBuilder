using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFanMount : BaseEntity
{
    public Guid ChassisId { get; private set; }
    public FanMountLocation Location { get; private set; }
    public bool SingleDiameterOnly { get; private set; }
    public Chassis Chassis { get; private set; } = null!;

    private readonly List<ChassisFanMountOption> _options = [];
    public IReadOnlyCollection<ChassisFanMountOption> Options => _options;
    
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
        if (_options.Any(x => x.Diameter == option.Diameter))
            throw new ArgumentException("Option already exists");
        
        _options.Add(option);
    }

    public void RemoveOption(ChassisFanMountOption option)
    {
        if (_options.All(x => x.Diameter != option.Diameter))
            throw new ArgumentException("Option does not exist");
        
        _options.Remove(option);
    }
    
    private void SetSpecs(Guid chassisId, FanMountLocation location, bool singleDiameterOnly)
    {
        if (chassisId == Guid.Empty)
            throw new ArgumentException("ChassisId must be a non-empty guid");
        
        if (!Enum.IsDefined(typeof(FanMountLocation), location))
            throw new ArgumentException("Invalid location");
        
        ChassisId = chassisId;
        Location = location;
        SingleDiameterOnly = singleDiameterOnly;
    }
}
