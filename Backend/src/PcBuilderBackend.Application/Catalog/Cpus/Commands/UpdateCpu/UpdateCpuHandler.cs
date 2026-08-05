using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

public class UpdateCpuHandler(IApplicationDbContext context, IMapper mapper, ILogger<UpdateCpuHandler> logger)
    : IRequestHandler<UpdateCpuCommand, CpuDto?>
{
    public async Task<CpuDto?> Handle(UpdateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = context.Cpus.FirstOrDefault(c => c.Id == request.Id && c.IsActive);
        if (entity == null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.SocketId,
            request.SeriesId,
            request.MaxMemoryGb,
            request.IntegratedGraphics,
            request.IncludedStockCooler,
            request.ThermalDesignPower);

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.Cpu, entity.Id);

        return mapper.Map<CpuDto>(entity);
    }
}
