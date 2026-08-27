using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;

public record WirelessNetworkAdapterFilter
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public WifiStandard? WifiStandard { get; init; }
    public BluetoothVersion? BluetoothVersion { get; init; }
    public WirelessHostInterface? HostInterface { get; init; }
    public RangeFilter? MaxSpeedMbps { get; init; }
    public RangeFilter? MaxSpeedMbps5G { get; init; }
    public RangeFilter? MaxSpeedMbps6G { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
    public M2Key? Key { get; init; }
    public M2FormFactor? M2FormFactor { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
    public Guid? MotherboardId { get; init; }
}
