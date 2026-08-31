using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

public class CreateGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries,
    ILogger<CreateGpuSeriesHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<CreateGpuSeriesCommand, GpuSeriesDto>
{
    public async Task<GpuSeriesDto> Handle(CreateGpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.GpuSeries(command.ManufacturerId, command.Name);
        gpuSeries.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Created(logger, EntityLog.GpuSeries, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return mapper.Map<GpuSeriesDto>(entity);
    }
}
