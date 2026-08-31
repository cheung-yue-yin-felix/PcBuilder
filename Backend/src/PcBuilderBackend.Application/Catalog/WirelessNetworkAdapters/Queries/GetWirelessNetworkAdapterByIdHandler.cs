using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class GetWirelessNetworkAdapterByIdHandler(IWirelessNetworkAdapterReadStore store)
    : IRequestHandler<GetWirelessNetworkAdapterByIdQuery, WirelessNetworkAdapterDto?>
{
    public Task<WirelessNetworkAdapterDto?> Handle(
        GetWirelessNetworkAdapterByIdQuery request,
        CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
