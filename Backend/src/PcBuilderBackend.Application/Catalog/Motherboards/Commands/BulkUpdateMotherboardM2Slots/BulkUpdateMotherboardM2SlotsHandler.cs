using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardM2Slots;

public class BulkUpdateMotherboardM2SlotsHandler(
    IMotherboardRepository motherboards,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateMotherboardM2SlotsHandler> logger)
    : IRequestHandler<BulkUpdateMotherboardM2SlotsCommand, List<MotherboardM2Dto>?>
{
    public async Task<List<MotherboardM2Dto>?> Handle(
        BulkUpdateMotherboardM2SlotsCommand request,
        CancellationToken cancellationToken)
    {
        var motherboard = await motherboards.GetWithChildrenAsync(request.MotherboardId, cancellationToken);

        if (motherboard is null)
        {
            EntityLog.NotFound(logger, EntityLog.Motherboard, request.MotherboardId);
            return null;
        }

        var existingByKey = motherboard.M2Slots.ToDictionary(x => x.GroupKey());
        var touchedKeys = new HashSet<MotherboardM2GroupKey>();

        foreach (var slot in request.M2Slots)
        {
            var identity = MotherboardM2GroupKey.From(
                slot.Key, slot.PcieGeneration, slot.SupportsSata, slot.FormFactors);
            touchedKeys.Add(identity);

            if (existingByKey.TryGetValue(identity, out var existing))
            {
                existing.UpdateSpecs(slot.Key, slot.PcieGeneration, slot.SlotCount, slot.SupportsSata);
                SyncFormFactors(existing, slot.FormFactors);
            }
            else
            {
                var m2 = new MotherboardM2(
                    request.MotherboardId, slot.Key, slot.PcieGeneration, slot.SlotCount, slot.SupportsSata);

                foreach (var formFactor in slot.FormFactors.Distinct())
                {
                    m2.AddFormFactor(new MotherboardM2FormFactor(m2.Id, formFactor));
                }

                motherboard.AddM2Slot(m2);
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.GroupKey())))
        {
            motherboard.RemoveM2Slot(existing);
            motherboards.DeleteM2Slot(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.MotherboardM2SlotsUpdated(logger, motherboard.Id);

        return [.. motherboard.M2Slots.Select(mapper.Map<MotherboardM2Dto>)];
    }

    private void SyncFormFactors(MotherboardM2 existing, IEnumerable<M2FormFactor> formFactors)
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
            motherboards.DeleteM2FormFactor(formFactorEntity);
        }
    }
}
