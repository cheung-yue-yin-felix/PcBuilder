using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WirelessNetworkAdapter : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public WifiStandard WifiStandard { get; set; }
    
    public BluetoothVersion? BluetoothVersion { get; set; }
    
    public WirelessHostInterface HostInterface { get; set; }
    
    public int MaxSpeedMbps { get; set; }
    
    public int? MaxSpeedMbps5G { get; set; }
    
    public int? MaxSpeedMbps6G { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}