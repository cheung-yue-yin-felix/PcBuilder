using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardsHandler(IMotherboardReadStore store)
    : IRequestHandler<GetMotherboardsQuery, PagedResult<MotherboardListItemDto>>
{
    public async Task<PagedResult<MotherboardListItemDto>> Handle(GetMotherboardsQuery query, CancellationToken cancellationToken)
    {
        return await store.ListAsync(query.Request, cancellationToken);
    }
}
