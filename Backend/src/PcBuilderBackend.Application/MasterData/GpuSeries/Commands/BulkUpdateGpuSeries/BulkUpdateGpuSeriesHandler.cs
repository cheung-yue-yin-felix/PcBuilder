using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkUpdateGpuSeries;

public class BulkUpdateGpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<BulkUpdateGpuSeriesCommand, List<GpuSeriesDto>>
{
    public async Task<List<GpuSeriesDto>> Handle(BulkUpdateGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<GpuSeriesDto>();

        foreach (var dto in request.GpuSeries)
        {
            var entity = await context.GpuSeries.FirstOrDefaultAsync(g => g.Id == dto.Id && g.IsActive, cancellationToken);

            if (entity is null) return result;

            entity.Rename(dto.Name);
            entity.UpdateManufacturer(dto.ManufacturerId);

            result.Add(mapper.Map<GpuSeriesDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return result;
    }
}
