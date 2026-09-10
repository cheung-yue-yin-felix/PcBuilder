using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.CreateWiredNetworkAdapter;

public record CreateWiredNetworkAdapterCommand : IRequest<WiredNetworkAdapterDto>, IWiredNetworkAdapterFields
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public WiredHostInterface HostInterface { get; init; }
    public int MaxSpeedMbps { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
}
