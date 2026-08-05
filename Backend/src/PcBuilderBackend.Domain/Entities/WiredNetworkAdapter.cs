using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WiredNetworkAdapter : ProductEntity
{
    public WiredHostInterface HostInterface { get; set; }
    public int MaxSpeedMbps { get; set; }
    
    protected WiredNetworkAdapter() { }

    public WiredNetworkAdapter(string name, Guid manufacturerId, WiredHostInterface hostInterface, int maxSpeedMbps)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(hostInterface, maxSpeedMbps);
    }

    public void UpdateSpecs(WiredHostInterface hostInterface, int maxSpeedMbps)
    {
        SetSpecs(hostInterface, maxSpeedMbps);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(WiredHostInterface hostInterface, int maxSpeedMbps)
    {
        if (!Enum.IsDefined(hostInterface))
            throw new ArgumentOutOfRangeException(nameof(hostInterface));
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps);
        
        HostInterface = hostInterface;
        MaxSpeedMbps = maxSpeedMbps;
    }
}
