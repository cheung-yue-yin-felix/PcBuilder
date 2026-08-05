using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpusHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetCpusQuery, PagedResult<CpuListItemDto>>
{
    public async Task<PagedResult<CpuListItemDto>> Handle(GetCpusQuery request, CancellationToken cancellationToken)
    {
        return await context.Cpus
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(request.Name), x => x.Name.Contains(request.Name!))
            .WhereIf(request.ManufacturerId.HasValue, x => x.ManufacturerId == request.ManufacturerId)
            .WhereIf(request.SocketId.HasValue, x => x.SocketId == request.SocketId)
            .WhereIf(request.SeriesId.HasValue, x => x.SeriesId == request.SeriesId)
            .WhereIf(
                request.DdrGeneration.HasValue,
                x => x.RamCompats.Any(r => r.IsActive && r.DdrGeneration == request.DdrGeneration))
            .Where(x => x.IsActive)
            .ToPagedResultAsync<Cpu, CpuListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
