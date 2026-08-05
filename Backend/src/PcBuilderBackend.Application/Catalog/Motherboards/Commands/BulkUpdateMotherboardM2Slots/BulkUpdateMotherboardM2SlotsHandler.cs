using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardM2Slots;

public class BulkUpdateMotherboardM2SlotsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateMotherboardM2SlotsHandler> logger)
    : IRequestHandler<BulkUpdateMotherboardM2SlotsCommand, List<MotherboardM2Dto>?>
{
    public async Task<List<MotherboardM2Dto>?> Handle(
        BulkUpdateMotherboardM2SlotsCommand request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .Include(x => x.M2Slots)
            .ThenInclude(x => x.FormFactors)
            .FirstOrDefaultAsync(x => x.Id == request.MotherboardId && x.IsActive, cancellationToken);

        if (motherboard is null)
        {
            EntityLog.NotFound(logger, EntityLog.Motherboard, request.MotherboardId);
            return null;
        }

        var existingByKey = motherboard.M2Slots.ToDictionary(x => x.PcieGeneration);
        var touchedKeys = new HashSet<PcieGeneration>();

        foreach (var slot in request.M2Slots)
        {
            touchedKeys.Add(slot.PcieGeneration);

            if (existingByKey.TryGetValue(slot.PcieGeneration, out var existing))
            {
                existing.UpdateSpecs(slot.PcieGeneration, slot.SlotCount);
                SyncFormFactors(existing, slot.FormFactors);
            }
            else
            {
                var m2 = new MotherboardM2(request.MotherboardId, slot.PcieGeneration, slot.SlotCount);

                foreach (var formFactor in slot.FormFactors.Distinct())
                {
                    m2.AddFormFactor(new MotherboardM2FormFactor(m2.Id, formFactor));
                }

                motherboard.AddM2Slot(m2);
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.PcieGeneration)))
        {
            motherboard.RemoveM2Slot(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.MotherboardM2SlotsUpdated(logger, motherboard.Id);

        return [.. motherboard.M2Slots.Select(mapper.Map<MotherboardM2Dto>)];
    }

    private static void SyncFormFactors(MotherboardM2 existing, IEnumerable<M2FormFactor> formFactors)
    {
        var requested = formFactors.Distinct().ToHashSet();
        var existingByKey = existing.FormFactors.ToDictionary(x => x.FormFactor);

        foreach (var formFactor in requested)
        {
            if (!existingByKey.ContainsKey(formFactor))
            {
                existing.AddFormFactor(new MotherboardM2FormFactor(existing.Id, formFactor));
            }
        }

        foreach (var formFactorEntity in existingByKey.Values.Where(x => !requested.Contains(x.FormFactor)))
        {
            existing.RemoveFormFactor(formFactorEntity);
        }
    }
}
