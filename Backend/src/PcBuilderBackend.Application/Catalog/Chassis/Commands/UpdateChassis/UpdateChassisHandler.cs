using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.UpdateChassis;

public class UpdateChassisHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<UpdateChassisHandler> logger)
    : IRequestHandler<UpdateChassisCommand, ChassisDto?>
{
    public async Task<ChassisDto?> Handle(UpdateChassisCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Chassis
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Chassis, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.LengthMm,
            request.WidthMm,
            request.HeightMm,
            request.MotherboardMaxWidthMm,
            request.MotherboardMaxHeightMm,
            request.MaxCpuCoolerHeightMm,
            request.MaxGraphicsCardLengthMm,
            request.MaxPsuLengthMm);

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.Chassis, entity.Id);

        return mapper.Map<ChassisDto>(entity);
    }
}
