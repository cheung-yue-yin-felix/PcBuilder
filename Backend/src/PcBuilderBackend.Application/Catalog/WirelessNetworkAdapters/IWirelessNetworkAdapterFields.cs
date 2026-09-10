using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;

public interface IWirelessNetworkAdapterFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    WifiStandard WifiStandard { get; }
    WirelessHostInterface HostInterface { get; }
    int MaxSpeedMbps { get; }
    int? MaxSpeedMbps5G { get; }
    int? MaxSpeedMbps6G { get; }
    BluetoothVersion? BluetoothVersion { get; }
    PcieSlotType? PcieSlotType { get; }
    M2Key? Key { get; }
    M2FormFactor? M2FormFactor { get; }
    UsbVersion? UsbVersion { get; }
    UsbType? UsbType { get; }
}
