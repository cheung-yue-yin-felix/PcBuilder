using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkUpdateGpus;

public class BulkUpdateGpusHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICacheService cache,
    ILogger<BulkUpdateGpusHandler> logger) : IRequestHandler<BulkUpdateGpusCommand, List<GpuDto>>
{
    public async Task<List<GpuDto>> Handle(BulkUpdateGpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<GpuDto>();

        foreach (var gpuDto in request.Gpus)
        {
            var gpu = await context.Gpus.FirstOrDefaultAsync(g => g.Id == gpuDto.Id && g.IsActive, cancellationToken);
            if (gpu is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.Gpu, gpuDto.Id);
                return result;
            }
            gpu.Rename(gpuDto.Name);
            gpu.UpdateManufacturer(gpuDto.ManufacturerId);
            gpu.UpdateSeries(gpuDto.GpuSeriesId);
            result.Add(mapper.Map<GpuDto>(gpu));
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Gpu);
        return result;
    }
}
