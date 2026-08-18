using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPcieSlots;

public class BulkUpdateChassisPcieSlotsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateChassisPcieSlotsHandler> logger)
    : IRequestHandler<BulkUpdateChassisPcieSlotsCommand, List<ChassisPcieSlotDto>?>
{
    public async Task<List<ChassisPcieSlotDto>?> Handle(
        BulkUpdateChassisPcieSlotsCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.PcieSlots)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = chassis.PcieSlots.ToDictionary(x => (x.LowProfileSlots, x.Orientation));
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
                chassis.AddPcieSlot(new ChassisPcieSlot(
                    request.ChassisId,
                    slot.LowProfileSlots,
                    slot.SlotCount,
                    slot.Orientation));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x =>
                     !touchedKeys.Contains((x.LowProfileSlots, x.Orientation))))
        {
            chassis.RemovePcieSlot(existing);
            context.ChassisPcieSlots.Remove(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisPcieSlotsUpdated(logger, chassis.Id);

        return [.. chassis.PcieSlots
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisPcieSlotDto>)];
    }
}
