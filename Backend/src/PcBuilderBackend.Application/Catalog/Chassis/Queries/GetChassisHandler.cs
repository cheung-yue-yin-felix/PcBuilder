using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisHandler(IChassisReadStore store): IRequestHandler<GetChassisQuery, PagedResult<ChassisListItemDto>>
{
    public async Task<PagedResult<ChassisListItemDto>> Handle(GetChassisQuery query, CancellationToken cancellationToken)
    {
        return await store.ListAsync(query.Request, cancellationToken);
    }
}