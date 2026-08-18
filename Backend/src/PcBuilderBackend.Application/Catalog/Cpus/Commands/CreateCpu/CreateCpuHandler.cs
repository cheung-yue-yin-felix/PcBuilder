using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;

public class CreateCpuHandler(IApplicationDbContext context, IMapper mapper, ILogger<CreateCpuHandler> logger)
    : IRequestHandler<CreateCpuCommand, CpuDto>
{
    public async Task<CpuDto> Handle(CreateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = new Cpu(
            request.Name,
            request.ManufacturerId,
            request.SocketId,
            request.SeriesId,
            request.MaxMemoryGb,
            request.IntegratedGraphics,
            request.IncludedStockCooler,
            request.ThermalDesignPower,
            request.PowerConsumptionWatts);

        foreach (var compat in request.RamCompats)
        {
            entity.AddRamCompat(new CpuRamCompat(
                entity.Id,
                compat.DdrGeneration,
                compat.RamModuleCount,
                compat.RamRank,
                compat.MaxSpeedMts));
        }

        foreach (var support in request.SupportChipsets)
        {
            entity.AddSupportedChipset(new CpuSupportChipset(
                entity.Id,
                support.ChipsetId,
                support.RequiresBiosUpdate));
        }

        context.Cpus.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.Cpu, entity.Id);

        return mapper.Map<CpuDto>(entity);
    }
}
