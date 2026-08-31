using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class GetWiredNetworkAdaptersHandler(IWiredNetworkAdapterReadStore store)
    : IRequestHandler<GetWiredNetworkAdaptersQuery, PagedResult<WiredNetworkAdapterDto>>
{
    public Task<PagedResult<WiredNetworkAdapterDto>> Handle(
        GetWiredNetworkAdaptersQuery query,
        CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
