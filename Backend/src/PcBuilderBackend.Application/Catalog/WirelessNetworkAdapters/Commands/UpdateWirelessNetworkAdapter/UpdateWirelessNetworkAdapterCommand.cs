using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.UpdateWirelessNetworkAdapter;

public record UpdateWirelessNetworkAdapterCommand : IRequest<WirelessNetworkAdapterDto?>, IWirelessNetworkAdapterFields
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public WifiStandard WifiStandard { get; init; }
    public WirelessHostInterface HostInterface { get; init; }
    public int MaxSpeedMbps { get; init; }
    public int? MaxSpeedMbps5G { get; init; }
    public int? MaxSpeedMbps6G { get; init; }
    public BluetoothVersion? BluetoothVersion { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
    public M2Key? Key { get; init; }
    public M2FormFactor? M2FormFactor { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
}
