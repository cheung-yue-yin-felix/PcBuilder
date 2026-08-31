using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkUpdateGpuSeries;

public class BulkUpdateGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries,
    ILogger<BulkUpdateGpuSeriesHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache)
    : IRequestHandler<BulkUpdateGpuSeriesCommand, List<GpuSeriesDto>>
{
    public async Task<List<GpuSeriesDto>> Handle(BulkUpdateGpuSeriesCommand command,
        CancellationToken cancellationToken)
    {
        var result = new List<GpuSeriesDto>();
        var ids = command.GpuSeries.Select(dto => dto.Id).ToList();
        var entities = await gpuSeries.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "Update", EntityLog.GpuSeries, ids.Count, entities.Count);
            return result;
        }

        foreach (var entity in entities)
        {
            var dto = command.GpuSeries.First(dto => dto.Id == entity.Id);
            entity.Rename(dto.Name);
            entity.UpdateManufacturer(dto.ManufacturerId);
            result.Add(mapper.Map<GpuSeriesDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.GpuSeries);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return result;
    }
}
