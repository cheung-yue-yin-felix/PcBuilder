using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpusHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetGpusQuery, PagedResult<GpuDto>>
{
    public async Task<PagedResult<GpuDto>> Handle(GetGpusQuery request, CancellationToken cancellationToken)
    {
        return await context.Gpus
            .AsNoTracking()
            .Where(x => x.IsActive)
            .WhereIf(request.ManufacturerId.HasValue, x => x.ManufacturerId == request.ManufacturerId)
            .WhereIf(request.GpuSeriesId.HasValue, x => x.SeriesId == request.GpuSeriesId)
            .WhereIf(!string.IsNullOrEmpty(request.Name), x => x.Name.Contains(request.Name!))
            .ToPagedResultAsync<Gpu, GpuDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}