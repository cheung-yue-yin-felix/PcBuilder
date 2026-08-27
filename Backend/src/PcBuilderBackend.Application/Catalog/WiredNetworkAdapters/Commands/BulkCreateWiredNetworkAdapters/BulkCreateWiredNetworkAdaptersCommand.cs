using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkCreateWiredNetworkAdapters;

public record BulkCreateWiredNetworkAdaptersCommand(List<CreateWiredNetworkAdapterItem> Adapters)
    : IRequest<List<WiredNetworkAdapterDto>>;

public record CreateWiredNetworkAdapterItem
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public WiredHostInterface HostInterface { get; init; }
    public int MaxSpeedMbps { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
}
