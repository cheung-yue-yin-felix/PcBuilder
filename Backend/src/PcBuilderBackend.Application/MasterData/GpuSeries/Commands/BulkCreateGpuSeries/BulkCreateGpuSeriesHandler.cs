using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkCreateGpuSeries;

public class BulkCreateGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries, 
    ILogger<BulkCreateGpuSeriesHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkCreateGpuSeriesCommand, List<GpuSeriesDto>>
{
    public async Task<List<GpuSeriesDto>> Handle(BulkCreateGpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var result = new List<GpuSeriesDto>();
        foreach (var entity in command.GpuSeries.Select(x => new Domain.Entities.GpuSeries(x.ManufacturerId, x.Name)))
        {
            gpuSeries.Add(entity);
            result.Add(mapper.Map<GpuSeriesDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.GpuSeries);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return result;
    }
}
