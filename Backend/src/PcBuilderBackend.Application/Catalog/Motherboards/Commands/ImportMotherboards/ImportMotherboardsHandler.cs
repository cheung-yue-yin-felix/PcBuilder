using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.ImportMotherboards;

public class ImportMotherboardsHandler(
    IApplicationDbContext context,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportMotherboardsHandler> logger)
    : IRequestHandler<ImportMotherboardsCommand, List<MotherboardDto>>
{
    public async Task<List<MotherboardDto>> Handle(
        ImportMotherboardsCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseMotherboardImportAsync(request.Stream, cancellationToken);
        var result = new List<Motherboard>();

        foreach (var row in rows)
        {
            var entity = new Motherboard(
                row.ManufacturerId,
                row.Name,
                row.SocketId,
                row.ChipsetId,
                row.RamSlots,
                row.MaxMemoryGb,
                row.MaxDimmSizeGb,
                row.SataPorts,
                row.FanConnectors,
                row.EpsConnectors,
                row.WidthMm,
                row.HeightMm,
                row.DdrGeneration,
                row.RamFormFactor,
                row.FormFactor,
                row.WifiEnabled,
                row.BluetoothEnabled);

            foreach (var slot in row.PcieSlots)
            {
                entity.AddPcieSlot(new MotherboardPcie(
                    entity.Id,
                    slot.SlotType,
                    slot.SlotLanes,
                    slot.Generation,
                    slot.SlotCount));
            }

            foreach (var slot in row.M2Slots)
            {
                var m2 = new MotherboardM2(entity.Id, slot.PcieGeneration, slot.SlotCount);

                foreach (var formFactor in slot.FormFactors)
                {
                    m2.AddFormFactor(new MotherboardM2FormFactor(m2.Id, formFactor));
                }

                entity.AddM2Slot(m2);
            }

            context.Motherboards.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.Motherboard);

        return [.. result.Select(mapper.Map<MotherboardDto>)];
    }
}
