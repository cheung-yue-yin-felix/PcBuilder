using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.UpdateWirelessNetworkAdapter;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkUpdateWirelessNetworkAdapters;

public record BulkUpdateWirelessNetworkAdaptersCommand(List<UpdateWirelessNetworkAdapterCommand> Adapters)
    : IRequest<List<WirelessNetworkAdapterDto>?>;
