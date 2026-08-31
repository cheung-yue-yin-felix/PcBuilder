using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class FilterWirelessNetworkAdaptersHandler(IWirelessNetworkAdapterReadStore store)
    : IRequestHandler<FilterWirelessNetworkAdaptersQuery, PagedResult<WirelessNetworkAdapterDto>>
{
    public Task<PagedResult<WirelessNetworkAdapterDto>> Handle(
        FilterWirelessNetworkAdaptersQuery query,
        CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
