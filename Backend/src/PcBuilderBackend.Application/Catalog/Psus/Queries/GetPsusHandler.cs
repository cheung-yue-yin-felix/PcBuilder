using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsusHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPsusQuery, PagedResult<PsuListItemDto>>
{
    public async Task<PagedResult<PsuListItemDto>> Handle(
        GetPsusQuery query,
        CancellationToken cancellationToken)
    {
        return await context.Psus
            .AsNoTracking()
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<Psu, PsuListItemDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
