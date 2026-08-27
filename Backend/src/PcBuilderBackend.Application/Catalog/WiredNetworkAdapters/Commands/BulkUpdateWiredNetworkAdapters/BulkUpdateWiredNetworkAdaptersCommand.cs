using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.UpdateWiredNetworkAdapter;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkUpdateWiredNetworkAdapters;

public record BulkUpdateWiredNetworkAdaptersCommand(List<UpdateWiredNetworkAdapterCommand> Adapters)
    : IRequest<List<WiredNetworkAdapterDto>?>;
