using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class FilterChassisHandler(IChassisReadStore store)
    : IRequestHandler<FilterChassisQuery, PagedResult<ChassisListItemDto>>
{
    public async Task<PagedResult<ChassisListItemDto>> Handle(FilterChassisQuery query,
        CancellationToken cancellationToken)
    {
        return await store.FilterAsync(query.Request, cancellationToken);
    }
}