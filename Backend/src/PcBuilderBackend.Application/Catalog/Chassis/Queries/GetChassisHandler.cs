using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetChassisQuery, PagedResult<ChassisListItemDto>>
{
    public async Task<PagedResult<ChassisListItemDto>> Handle(GetChassisQuery request, CancellationToken cancellationToken)
    {
        return await context.Chassis
            .AsNoTracking()
            .ApplySorting(request.Request.SortFields, request.Request.SortDirection)
            .ToPagedResultAsync<Domain.Entities.Chassis, ChassisListItemDto>(
                request.Request.PageIndex,
                request.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}