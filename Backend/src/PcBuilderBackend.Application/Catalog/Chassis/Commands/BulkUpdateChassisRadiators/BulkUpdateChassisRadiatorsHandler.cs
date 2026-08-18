using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisRadiators;

public class BulkUpdateChassisRadiatorsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateChassisRadiatorsHandler> logger)
    : IRequestHandler<BulkUpdateChassisRadiatorsCommand, List<ChassisRadiatorDto>?>
{
    public async Task<List<ChassisRadiatorDto>?> Handle(
        BulkUpdateChassisRadiatorsCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.Radiators)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = chassis.Radiators.ToDictionary(x => (x.Length, x.MountLocation));
        var touchedKeys = new HashSet<(RadiatorLength Length, RadiatorMountLocation MountLocation)>();

        foreach (var radiator in request.Radiators)
        {
            var key = (radiator.Length, radiator.Location);
            touchedKeys.Add(key);

            if (existingByKey.TryGetValue(key, out var existing))
            {
                existing.UpdateSpecs(
                    request.ChassisId,
                    radiator.Length,
                    radiator.Location,
                    radiator.RadiatorCount);
            }
            else
            {
                chassis.AddRadiator(new ChassisRadiator(
                    request.ChassisId,
                    radiator.Length,
                    radiator.Location,
                    radiator.RadiatorCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x =>
                     !touchedKeys.Contains((x.Length, x.MountLocation))))
        {
            chassis.RemoveRadiator(existing);
            context.ChassisRadiators.Remove(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisRadiatorsUpdated(logger, chassis.Id);

        return [.. chassis.Radiators
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisRadiatorDto>)];
    }
}
