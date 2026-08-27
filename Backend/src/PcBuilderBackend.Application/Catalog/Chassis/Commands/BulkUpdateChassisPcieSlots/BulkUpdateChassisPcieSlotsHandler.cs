using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPcieSlots;

public class BulkUpdateChassisPcieSlotsHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateChassisPcieSlotsHandler> logger)
    : IRequestHandler<BulkUpdateChassisPcieSlotsCommand, List<ChassisPcieSlotDto>?>
{
    public async Task<List<ChassisPcieSlotDto>?> Handle(
        BulkUpdateChassisPcieSlotsCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await chassis.GetWithChildrenAsync(request.ChassisId, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = entity.PcieSlots.ToDictionary(x => (x.LowProfileSlots, x.Orientation));
        var touchedKeys = new HashSet<(bool LowProfileSlots, PcieOrientation Orientation)>();

        foreach (var slot in request.PcieSlots)
        {
            var key = (slot.LowProfileSlots, slot.Orientation);
            touchedKeys.Add(key);

            if (existingByKey.TryGetValue(key, out var existing))
            {
                existing.UpdateSpecs(
                    request.ChassisId,
                    slot.LowProfileSlots,
                    slot.SlotCount,
                    slot.Orientation);
            }
            else
            {
                entity.AddPcieSlot(new ChassisPcieSlot(
                    request.ChassisId,
                    slot.LowProfileSlots,
                    slot.SlotCount,
                    slot.Orientation));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x =>
                     !touchedKeys.Contains((x.LowProfileSlots, x.Orientation))))
        {
            entity.RemovePcieSlot(existing);
            chassis.DeletePcieSlot(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisPcieSlotsUpdated(logger, entity.Id);

        return [.. entity.PcieSlots
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisPcieSlotDto>)];
    }
}
