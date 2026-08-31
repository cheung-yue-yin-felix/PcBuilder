using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;

public class UpdateGpuHandler(
    IRepository<Gpu> gpus, 
    ILogger<UpdateGpuHandler> logger, 
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<UpdateGpuCommand, GpuDto?>
{
    public async Task<GpuDto?> Handle(UpdateGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = await gpus.GetByIdAsync(request.Id, cancellationToken);
        if (gpu is null) return null;

        gpu.Rename(request.Name);
        gpu.UpdateSeries(request.GpuSeriesId);
        gpu.UpdateManufacturer(request.ManufacturerId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Updated(logger, EntityLog.Gpu, gpu.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        return mapper.Map<GpuDto>(gpu);
    }
}
