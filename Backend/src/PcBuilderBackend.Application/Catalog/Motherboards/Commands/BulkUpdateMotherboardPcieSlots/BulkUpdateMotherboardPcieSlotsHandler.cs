using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardPcieSlots;

public class BulkUpdateMotherboardPcieSlotsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateMotherboardPcieSlotsHandler> logger)
    : IRequestHandler<BulkUpdateMotherboardPcieSlotsCommand, List<MotherboardPcieDto>?>
{
    public async Task<List<MotherboardPcieDto>?> Handle(
        BulkUpdateMotherboardPcieSlotsCommand request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .Include(x => x.PcieSlots)
            .FirstOrDefaultAsync(x => x.Id == request.MotherboardId && x.IsActive, cancellationToken);

        if (motherboard is null)
        {
            EntityLog.NotFound(logger, EntityLog.Motherboard, request.MotherboardId);
            return null;
        }

        var existingByKey = motherboard.PcieSlots.ToDictionary(
            x => (x.SlotType, x.SlotLanes, x.Generation));

        var touchedKeys = new HashSet<(PcieSlotType, PcieSlotLane, PcieGeneration)>();

        foreach (var slot in request.PcieSlots)
        {
            var key = (slot.SlotType, slot.SlotLanes, slot.Generation);
            touchedKeys.Add(key);

            if (existingByKey.TryGetValue(key, out var existing))
            {
                existing.UpdateSpecs(slot.SlotType, slot.SlotLanes, slot.Generation, slot.SlotCount);
            }
            else
            {
                motherboard.AddPcieSlot(new MotherboardPcie(
                    request.MotherboardId,
                    slot.SlotType,
                    slot.SlotLanes,
                    slot.Generation,
                    slot.SlotCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x =>
                     !touchedKeys.Contains((x.SlotType, x.SlotLanes, x.Generation))))
        {
            motherboard.RemovePcieSlot(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.MotherboardPcieSlotsUpdated(logger, motherboard.Id);

        return [.. motherboard.PcieSlots.Select(mapper.Map<MotherboardPcieDto>)];
    }
}
