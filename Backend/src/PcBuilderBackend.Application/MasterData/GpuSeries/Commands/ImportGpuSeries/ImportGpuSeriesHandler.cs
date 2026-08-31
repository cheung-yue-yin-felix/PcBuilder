using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.ImportGpuSeries;

public class ImportGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportGpuSeriesHandler> logger)
    : IRequestHandler<ImportGpuSeriesCommand, List<GpuSeriesDto>>
{
    public async Task<List<GpuSeriesDto>> Handle(
        ImportGpuSeriesCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseGpuSeriesImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<Domain.Entities.GpuSeries>();

        foreach (var row in rows)
        {
            var entity = new Domain.Entities.GpuSeries(row.ManufacturerId, row.Name);
            gpuSeries.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.GpuSeries);

        return [.. result.Select(mapper.Map<GpuSeriesDto>)];
    }
}
