using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class FilterWiredNetworkAdaptersHandler(IWiredNetworkAdapterReadStore store)
    : IRequestHandler<FilterWiredNetworkAdaptersQuery, PagedResult<WiredNetworkAdapterDto>>
{
    public Task<PagedResult<WiredNetworkAdapterDto>> Handle(
        FilterWiredNetworkAdaptersQuery query,
        CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
