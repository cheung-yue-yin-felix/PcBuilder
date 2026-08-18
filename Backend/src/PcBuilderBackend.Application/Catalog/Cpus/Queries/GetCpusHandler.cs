using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpusHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetCpusQuery, PagedResult<CpuListItemDto>>
{
    public async Task<PagedResult<CpuListItemDto>> Handle(GetCpusQuery query, CancellationToken cancellationToken)
    {
        return await context.Cpus
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Include(x => x.Socket)
            .Include(x => x.Series)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<Cpu, CpuListItemDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
