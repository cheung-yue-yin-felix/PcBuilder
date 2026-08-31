using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;

public class ImportGpusHandler(
    IRepository<Gpu> gpus,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportGpusHandler> logger)
    : IRequestHandler<ImportGpusCommand, List<GpuDto>>
{
    public async Task<List<GpuDto>> Handle(ImportGpusCommand request, CancellationToken cancellationToken)
    {
        var entities = await excel.ParseGpuImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(lookup, entities.Select(g => g.ManufacturerId), cancellationToken);
        await ActiveEntityGuard.EnsureGpuSeriesExist(lookup, entities.Select(g => g.SeriesId), cancellationToken);

        var result = new List<Gpu>();

        foreach (var gpu in entities)
        {
            var entity = new Gpu(gpu.Name, gpu.ManufacturerId, gpu.SeriesId);
            gpus.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.Gpu);

        return [.. result.Select(mapper.Map<GpuDto>)];
    }
}
