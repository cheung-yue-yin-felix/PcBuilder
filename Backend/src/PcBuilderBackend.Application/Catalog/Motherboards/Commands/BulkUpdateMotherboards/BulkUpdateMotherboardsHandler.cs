using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboards;

public class BulkUpdateMotherboardsHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<BulkUpdateMotherboardsCommand, List<MotherboardDto>>
{
    public async Task<List<MotherboardDto>> Handle(BulkUpdateMotherboardsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<MotherboardDto>();

        foreach (var dto in request.Motherboards)
        {
            var entity = await context.Motherboards.FirstOrDefaultAsync(m => m.Id == dto.Id && m.IsActive, cancellationToken);
            if (entity is null) return result;

            entity.Rename(dto.Name);
            entity.UpdateManufacturer(dto.ManufacturerId);
            entity.UpdateSpecs(
                dto.SocketId,
                dto.ChipsetId,
                dto.RamSlots,
                dto.MaxMemoryGb,
                dto.MaxDimmSizeGb,
                dto.SataPorts,
                dto.FanConnectors,
                dto.EpsConnectors,
                dto.WidthMm,
                dto.HeightMm,
                dto.DdrGeneration,
                dto.RamFormFactor,
                dto.FormFactor,
                dto.WifiEnabled,
                dto.BluetoothEnabled
            );

            entity.PcieSlots.Clear();
            entity.M2Slots.Clear();

            foreach (var slot in dto.PcieSlots)
            {
                entity.AddPcieSlot(
                    new MotherboardPcie(
                        entity.Id,
                        slot.SlotType,
                        slot.SlotLanes,
                        slot.Generation,
                        slot.SlotCount
                    )
                );
            }
            
            foreach (var slot in dto.M2Slots)
            {
                var m2 = new MotherboardM2(
                    entity.Id,
                    slot.PcieGeneration,
                    slot.SlotCount
                );
                
                foreach (var formFactor in slot.FormFactors)
                {
                    m2.AddFormFactor(new MotherboardM2FormFactor(m2.Id, formFactor));
                }

                entity.AddM2Slot(m2);
            }
            
            result.Add(mapper.Map<MotherboardDto>(entity));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}