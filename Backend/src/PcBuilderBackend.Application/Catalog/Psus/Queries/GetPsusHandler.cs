using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsusHandler(IPsuReadStore store)
    : IRequestHandler<GetPsusQuery, PagedResult<PsuListItemDto>>
{
    public Task<PagedResult<PsuListItemDto>> Handle(GetPsusQuery query, CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
