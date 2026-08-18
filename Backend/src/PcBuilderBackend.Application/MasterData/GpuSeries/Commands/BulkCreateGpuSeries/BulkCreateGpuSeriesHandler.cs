using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkCreateGpuSeries;

public class BulkCreateGpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<BulkCreateGpuSeriesCommand, List<GpuSeriesDto>>
{
    public async Task<List<GpuSeriesDto>> Handle(BulkCreateGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<GpuSeriesDto>();
        foreach (var entity in request.GpuSeries.Select(x => new Domain.Entities.GpuSeries(x.ManufacturerId, x.Name)))
        {
            context.GpuSeries.Add(entity);
            result.Add(mapper.Map<GpuSeriesDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return result;
    }
}
