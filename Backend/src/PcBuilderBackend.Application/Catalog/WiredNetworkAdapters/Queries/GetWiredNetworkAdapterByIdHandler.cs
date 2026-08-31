using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class GetWiredNetworkAdapterByIdHandler(IWiredNetworkAdapterReadStore store)
    : IRequestHandler<GetWiredNetworkAdapterByIdQuery, WiredNetworkAdapterDto?>
{
    public Task<WiredNetworkAdapterDto?> Handle(
        GetWiredNetworkAdapterByIdQuery request,
        CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
