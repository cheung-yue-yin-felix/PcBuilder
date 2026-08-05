using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;

public class BulkCreateCpusHandler(IApplicationDbContext context, IMapper mapper, ILogger<BulkCreateCpusHandler> logger)
    : IRequestHandler<BulkCreateCpusCommand, List<CpuDto>>
{
    public async Task<List<CpuDto>> Handle(BulkCreateCpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<CpuDto>();

        foreach (var cpuDto in request.Cpus)
        {
            var entity = new Cpu(
                cpuDto.Name,
                cpuDto.ManufacturerId,
                cpuDto.SocketId,
                cpuDto.SeriesId,
                cpuDto.MaxMemoryGb,
                cpuDto.IntegratedGraphics,
                cpuDto.IncludedStockCooler,
                cpuDto.ThermalDesignPower);

            foreach (var compat in cpuDto.RamCompats)
            {
                entity.AddRamCompat(new CpuRamCompat(
                    entity.Id,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts));
            }

            context.Cpus.Add(entity);
            result.Add(mapper.Map<CpuDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.BulkCreated(logger, result.Count, EntityLog.Cpu);

        return result;
    }
}
