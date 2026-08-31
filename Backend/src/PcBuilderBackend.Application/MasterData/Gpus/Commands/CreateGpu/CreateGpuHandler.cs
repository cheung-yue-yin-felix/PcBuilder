using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;

public class CreateGpuHandler(
    IRepository<Domain.Entities.Gpu> gpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache,
    ILogger<CreateGpuHandler> logger) : IRequestHandler<CreateGpuCommand, GpuDto>
{
    public async Task<GpuDto> Handle(CreateGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = new Domain.Entities.Gpu(request.Name, request.ManufacturerId, request.GpuSeriesId);
        gpus.Add(gpu);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.Created(logger, EntityLog.Gpu, gpu.Id);
        return mapper.Map<GpuDto>(gpu);
    }
}
