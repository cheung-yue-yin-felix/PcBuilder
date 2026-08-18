using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.ImportCpu;

public class ImportCpusHandler(
    IApplicationDbContext context,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportCpusHandler> logger) : IRequestHandler<ImportCpusCommand, List<CpuDto>>
{
    public async Task<List<CpuDto>> Handle(ImportCpusCommand request, CancellationToken cancellationToken)
    {
        var cpus = await excel.ParseCpuImportAsync(request.Stream, cancellationToken);
        var result = new List<Cpu>();

        foreach (var cpu in cpus)
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

            context.Cpus.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.Cpu);

        return [.. result.Select(mapper.Map<CpuDto>)];
    }
}
