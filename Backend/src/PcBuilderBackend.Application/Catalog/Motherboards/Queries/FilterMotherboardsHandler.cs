using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class FilterMotherboardsHandler(IMotherboardReadStore store)
    : IRequestHandler<FilterMotherboardsQuery, PagedResult<MotherboardListItemDto>>
{
    public async Task<PagedResult<MotherboardListItemDto>> Handle(
        FilterMotherboardsQuery query,
        CancellationToken cancellationToken)
    {
        return await store.FilterAsync(query.Request, cancellationToken);
    }
}
