using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;

public class BulkCreateCpusHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkCreateCpusHandler> logger)
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
                new CpuSpecs
                {
                    SocketId = cpuDto.SocketId,
                    SeriesId = cpuDto.SeriesId,
                    MaxMemoryGb = cpuDto.MaxMemoryGb,
                    IntegratedGraphics = cpuDto.IntegratedGraphics,
                    IncludedStockCooler = cpuDto.IncludedStockCooler,
                    ThermalDesignPower = cpuDto.ThermalDesignPower,
                    PowerConsumptionWatts = cpuDto.PowerConsumptionWatts
                });

            foreach (var compat in cpuDto.RamCompats)
            {
                entity.AddRamCompat(new CpuRamCompat(
                    entity.Id,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts));
            }

            foreach (var support in cpuDto.SupportChipsets)
            {
                entity.AddSupportedChipset(new CpuSupportChipset(
                    entity.Id,
                    support.ChipsetId,
                    support.RequiresBiosUpdate));
            }

            cpus.Add(entity);
            result.Add(mapper.Map<CpuDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.BulkCreated(logger, result.Count, EntityLog.Cpu);

        return result;
    }
}
