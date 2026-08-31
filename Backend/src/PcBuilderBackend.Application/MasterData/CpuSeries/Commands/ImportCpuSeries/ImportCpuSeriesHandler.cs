using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.ImportCpuSeries;

public class ImportCpuSeriesHandler(
    IRepository<Domain.Entities.CpuSeries> cpuSeries,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportCpuSeriesHandler> logger)
    : IRequestHandler<ImportCpuSeriesCommand, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(
        ImportCpuSeriesCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseCpuSeriesImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);
        await ActiveEntityGuard.EnsureSocketsExist(
            lookup, rows.Select(r => r.SocketId), cancellationToken);

        var result = new List<Domain.Entities.CpuSeries>();

        foreach (var row in rows)
        {
            var entity = new Domain.Entities.CpuSeries(row.ManufacturerId, row.SocketId, row.Name);
            cpuSeries.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.CpuSeries);

        return [.. result.Select(mapper.Map<CpuSeriesDto>)];
    }
}
