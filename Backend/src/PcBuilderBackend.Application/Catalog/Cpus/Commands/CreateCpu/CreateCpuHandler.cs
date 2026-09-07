using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;

public class CreateCpuHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateCpuHandler> logger)
    : IRequestHandler<CreateCpuCommand, CpuDto>
{
    public async Task<CpuDto> Handle(CreateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = new Cpu(
            request.Name,
            request.ManufacturerId,
            new CpuSpecs
            {
                SocketId = request.SocketId,
                SeriesId = request.SeriesId,
                MaxMemoryGb = request.MaxMemoryGb,
                IntegratedGraphics = request.IntegratedGraphics,
                IncludedStockCooler = request.IncludedStockCooler,
                ThermalDesignPower = request.ThermalDesignPower,
                PowerConsumptionWatts = request.PowerConsumptionWatts
            });

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

        cpus.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.Cpu, entity.Id);

        return mapper.Map<CpuDto>(entity);
    }
}
