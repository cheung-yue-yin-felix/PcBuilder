using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.ImportCpu;

public class ImportCpusHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportCpusHandler> logger) : IRequestHandler<ImportCpusCommand, List<CpuDto>>
{
    public async Task<List<CpuDto>> Handle(ImportCpusCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParseCpuImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(lookup, rows.Select(c => c.ManufacturerId), cancellationToken);
        await ActiveEntityGuard.EnsureSocketsExist(lookup, rows.Select(c => c.SocketId), cancellationToken);
        await ActiveEntityGuard.EnsureCpuSeriesExist(lookup, rows.Select(c => c.SeriesId), cancellationToken);
        await ActiveEntityGuard.EnsureChipsetsExist(
            lookup,
            rows.SelectMany(c => c.SupportChipsets.Select(s => s.ChipsetId)),
            cancellationToken);

        var result = new List<Cpu>();

        foreach (var cpu in rows)
        {
            var entity = new Cpu(
                cpu.Name,
                cpu.ManufacturerId,
                cpu.SocketId,
                cpu.SeriesId,
                cpu.MaxMemoryGb,
                cpu.IntegratedGraphics,
                cpu.IncludedStockCooler,
                cpu.ThermalDesignPower,
                cpu.PowerConsumptionWatts);

            foreach (var compat in cpu.RamCompats)
            {
                entity.AddRamCompat(new CpuRamCompat(
                    entity.Id,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts));
            }

            foreach (var support in cpu.SupportChipsets)
            {
                entity.AddSupportedChipset(new CpuSupportChipset(
                    entity.Id,
                    support.ChipsetId,
                    support.RequiresBiosUpdate));
            }

            cpus.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.Cpu);

        return [.. result.Select(mapper.Map<CpuDto>)];
    }
}
