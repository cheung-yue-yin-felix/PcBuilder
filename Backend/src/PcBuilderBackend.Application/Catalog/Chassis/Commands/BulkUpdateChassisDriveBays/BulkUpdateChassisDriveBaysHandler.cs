using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisDriveBays;

public class BulkUpdateChassisDriveBaysHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateChassisDriveBaysHandler> logger)
    : IRequestHandler<BulkUpdateChassisDriveBaysCommand, List<ChassisDriveBayDto>?>
{
    public async Task<List<ChassisDriveBayDto>?> Handle(
        BulkUpdateChassisDriveBaysCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.DriveBays)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = chassis.DriveBays.ToDictionary(x => x.DriveBayFormFactor);
        var touchedKeys = new HashSet<DriveBayFormFactor>();

        foreach (var bay in request.DriveBays)
        {
            touchedKeys.Add(bay.FormFactor);

            if (existingByKey.TryGetValue(bay.FormFactor, out var existing))
            {
                existing.UpdateSpecs(request.ChassisId, bay.FormFactor, bay.SlotCount);
            }
            else
            {
                chassis.AddDriveBay(new ChassisDriveBay(
                    request.ChassisId,
                    bay.FormFactor,
                    bay.SlotCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.DriveBayFormFactor)))
        {
            chassis.RemoveDriveBay(existing);
            context.ChassisDriveBays.Remove(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisDriveBaysUpdated(logger, chassis.Id);

        return [.. chassis.DriveBays
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisDriveBayDto>)];
    }
}
