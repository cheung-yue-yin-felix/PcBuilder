using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

public class UpdateGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries,
    ILogger<UpdateGpuSeriesHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache)
    : IRequestHandler<UpdateGpuSeriesCommand, GpuSeriesDto?>
{
    public async Task<GpuSeriesDto?> Handle(UpdateGpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = await gpuSeries.GetByIdAsync(command.Id, cancellationToken);
        if (entity == null) return null;

        entity.Rename(command.Name);
        entity.UpdateManufacturer(command.ManufacturerId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Updated(logger, EntityLog.GpuSeries, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return mapper.Map<GpuSeriesDto>(entity);
    }
}
