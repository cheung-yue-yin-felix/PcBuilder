using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisDriveBays;

public class BulkUpdateChassisDriveBaysHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateChassisDriveBaysHandler> logger)
    : IRequestHandler<BulkUpdateChassisDriveBaysCommand, List<ChassisDriveBayDto>?>
{
    public async Task<List<ChassisDriveBayDto>?> Handle(
        BulkUpdateChassisDriveBaysCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await chassis.GetWithChildrenAsync(request.ChassisId, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = entity.DriveBays.ToDictionary(x => x.DriveBayFormFactor);
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
                entity.AddDriveBay(new ChassisDriveBay(
                    request.ChassisId,
                    bay.FormFactor,
                    bay.SlotCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.DriveBayFormFactor)))
        {
            entity.RemoveDriveBay(existing);
            chassis.DeleteDriveBay(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisDriveBaysUpdated(logger, entity.Id);

        return [.. entity.DriveBays
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisDriveBayDto>)];
    }
}
