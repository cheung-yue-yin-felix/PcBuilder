using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WirelessNetworkAdapter : ProductEntity
{
    public WifiStandard WifiStandard { get; set; }
    public BluetoothVersion? BluetoothVersion { get; set; }
    public WirelessHostInterface HostInterface { get; set; }
    public int MaxSpeedMbps { get; set; }
    public int? MaxSpeedMbps5G { get; set; }
    public int? MaxSpeedMbps6G { get; set; }
    
    protected WirelessNetworkAdapter() {}

    public WirelessNetworkAdapter(
        string name,
        Guid manufacturerId,
        WifiStandard wifiStandard, 
        WirelessHostInterface hostInterface, 
        int maxSpeedMbps, 
        int? maxSpeedMbps5G, 
        int? maxSpeedMbps6G, 
        BluetoothVersion? bluetoothVersion)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(wifiStandard, hostInterface, maxSpeedMbps, maxSpeedMbps5G, maxSpeedMbps6G, bluetoothVersion);
    }

    public void UpdateSpecs(
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion)
    {
        SetSpecs(wifiStandard, hostInterface, maxSpeedMbps, maxSpeedMbps5G, maxSpeedMbps6G, bluetoothVersion);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSpecs(
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion)
    {
        if (!Enum.IsDefined(wifiStandard))
            throw new ArgumentException("Wifi Standard is invalid");
        
        if (!Enum.IsDefined(hostInterface))
            throw new ArgumentException("Host interface is invalid");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps);
        
        if (maxSpeedMbps5G.HasValue)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps5G.Value);
        
        if (maxSpeedMbps6G.HasValue)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps6G.Value);
        
        if (bluetoothVersion.HasValue && !Enum.IsDefined(bluetoothVersion.Value))
            throw new ArgumentException("Bluetooth version is invalid");
        
        WifiStandard = wifiStandard;
        HostInterface = hostInterface;
        MaxSpeedMbps = maxSpeedMbps;
        MaxSpeedMbps5G = maxSpeedMbps5G;
        MaxSpeedMbps6G = maxSpeedMbps6G;
        BluetoothVersion = bluetoothVersion;
    }
}
