using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class WirelessNetworkAdapterSpecs
{
    public WifiStandard WifiStandard { get; init; }
    public WirelessHostInterface HostInterface { get; init; }
    public int MaxSpeedMbps { get; init; }
    public int? MaxSpeedMbps5G { get; init; }
    public int? MaxSpeedMbps6G { get; init; }
    public BluetoothVersion? BluetoothVersion { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
    public M2Key? M2Key { get; init; }
    public M2FormFactor? M2FormFactor { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
}
