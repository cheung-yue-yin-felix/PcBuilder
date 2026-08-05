using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpus;

public class BulkUpdateCpusHandler(IApplicationDbContext context, IMapper mapper, ILogger<BulkUpdateCpusHandler> logger)
    : IRequestHandler<BulkUpdateCpusCommand, List<CpuDto>?>
{
    public async Task<List<CpuDto>?> Handle(BulkUpdateCpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<CpuDto>();

        foreach (var cpuDto in request.Cpus)
        {
            var entity = await context.Cpus
                .Include(cpu => cpu.RamCompats)
                .FirstOrDefaultAsync(cpu => cpu.Id == cpuDto.Id && cpu.IsActive, cancellationToken);

            if (entity == null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, cpuDto.Id);
                return null;
            }

            entity.Rename(cpuDto.Name);
            entity.UpdateManufacturer(cpuDto.ManufacturerId);
            entity.UpdateSpecs(
                cpuDto.SocketId,
                cpuDto.SeriesId,
                cpuDto.MaxMemoryGb,
                cpuDto.IntegratedGraphics,
                cpuDto.IncludedStockCooler,
                cpuDto.ThermalDesignPower);

            entity.RamCompats.Clear();

            foreach (var compat in cpuDto.RamCompats)
            {
                entity.AddRamCompat(new CpuRamCompat(
                    entity.Id,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts));
            }

            result.Add(mapper.Map<CpuDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Cpu);

        return result;
    }
}
