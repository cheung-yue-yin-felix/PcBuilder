using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class GetChassisFansHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetChassisFansQuery, PagedResult<ChassisFanDto>>
{
    public async Task<PagedResult<ChassisFanDto>> Handle(GetChassisFansQuery query, CancellationToken cancellationToken)
    {
        return await context.ChassisFans
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<ChassisFan, ChassisFanDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken
            );
    }
}