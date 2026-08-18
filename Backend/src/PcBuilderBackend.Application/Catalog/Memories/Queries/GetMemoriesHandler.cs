using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMemoriesQuery, PagedResult<RamDto>>
{
    public async Task<PagedResult<RamDto>> Handle(GetMemoriesQuery query, CancellationToken cancellationToken)
    {
        return await context.Rams
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<Ram, RamDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
