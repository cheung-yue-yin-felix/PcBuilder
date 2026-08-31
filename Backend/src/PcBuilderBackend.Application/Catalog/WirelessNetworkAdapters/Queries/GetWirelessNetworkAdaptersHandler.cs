using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class GetWirelessNetworkAdaptersHandler(IWirelessNetworkAdapterReadStore store)
    : IRequestHandler<GetWirelessNetworkAdaptersQuery, PagedResult<WirelessNetworkAdapterDto>>
{
    public Task<PagedResult<WirelessNetworkAdapterDto>> Handle(
        GetWirelessNetworkAdaptersQuery query,
        CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
