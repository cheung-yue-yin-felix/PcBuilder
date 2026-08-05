using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkCreateMotherboards;

public class BulkCreateMotherboardsHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<BulkCreateMotherboardCommand, List<MotherboardDto>>
{
    public async Task<List<MotherboardDto>> Handle(BulkCreateMotherboardCommand request, CancellationToken cancellationToken)
    {
        var result = new List<MotherboardDto>();

        foreach (var motherboardDto in request.Motherboards)
        {
            var motherboard = new Motherboard(
                motherboardDto.ManufacturerId,
                motherboardDto.Name,
                motherboardDto.SocketId,
                motherboardDto.ChipsetId,
                motherboardDto.RamSlots,
                motherboardDto.MaxMemoryGb,
                motherboardDto.MaxDimmSizeGb,
                motherboardDto.SataPorts,
                motherboardDto.FanConnectors,
                motherboardDto.EpsConnectors,
                motherboardDto.WidthMm,
                motherboardDto.HeightMm,
                motherboardDto.DdrGeneration,
                motherboardDto.RamFormFactor,
                motherboardDto.FormFactor,
                motherboardDto.WifiEnabled,
                motherboardDto.BluetoothEnabled
            );

            foreach (var slot in motherboardDto.PcieSlots)
            {
                motherboard.AddPcieSlot(
                    new MotherboardPcie(
                        motherboard.Id,
                        slot.SlotType,
                        slot.SlotLanes,
                        slot.Generation,
                        slot.SlotCount
                    )
                );
            }

            foreach (var slot in motherboardDto.M2Slots)
            {
                var m2 = new MotherboardM2(
                    motherboard.Id,
                    slot.PcieGeneration,
                    slot.SlotCount
                );
                
                foreach (var formFactor in slot.FormFactors)
                {
                    m2.AddFormFactor(new MotherboardM2FormFactor(m2.Id, formFactor));
                }

                motherboard.AddM2Slot(m2);
            }

            context.Motherboards.Add(motherboard);

            result.Add(mapper.Map<MotherboardDto>(motherboard));
        }

        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}