using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class FilterPsusHandler(IPsuReadStore store)
    : IRequestHandler<FilterPsusQuery, PagedResult<PsuListItemDto>>
{
    public Task<PagedResult<PsuListItemDto>> Handle(FilterPsusQuery query, CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
