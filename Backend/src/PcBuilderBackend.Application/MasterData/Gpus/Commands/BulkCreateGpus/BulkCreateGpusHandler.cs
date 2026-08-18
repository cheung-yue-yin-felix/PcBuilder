using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;

public class BulkCreateGpusHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICacheService cache,
    ILogger<BulkCreateGpusHandler> logger)
    : IRequestHandler<BulkCreateGpusCommand, List<GpuDto>>
{
    public async Task<List<GpuDto>> Handle(BulkCreateGpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<GpuDto>();

        foreach (var gpu in request.Gpus.Select(g => new Domain.Entities.Gpu(g.Name, g.ManufacturerId, g.GpuSeriesId)))
        {
            context.Gpus.Add(gpu);
            result.Add(mapper.Map<GpuDto>(gpu));
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Gpu);
        return result;
    }
}
