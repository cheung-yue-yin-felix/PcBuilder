using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

public class UpdateCpuHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateCpuHandler> logger)
    : IRequestHandler<UpdateCpuCommand, CpuDto?>
{
    public async Task<CpuDto?> Handle(UpdateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = await cpus.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(new CpuSpecs
        {
            SocketId = request.SocketId,
            SeriesId = request.SeriesId,
            MaxMemoryGb = request.MaxMemoryGb,
            IntegratedGraphics = request.IntegratedGraphics,
            IncludedStockCooler = request.IncludedStockCooler,
            ThermalDesignPower = request.ThermalDesignPower,
            PowerConsumptionWatts = request.PowerConsumptionWatts
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.Cpu, entity.Id);

        return mapper.Map<CpuDto>(entity);
    }
}
