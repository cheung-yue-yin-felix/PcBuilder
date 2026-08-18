using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.CreateMotherboard;

public class CreateMotherboardHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateMotherboardCommand, MotherboardDto>
{
    public async Task<MotherboardDto> Handle(CreateMotherboardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Motherboard(
            request.ManufacturerId,
            request.Name,
            request.SocketId,
            request.ChipsetId,
            request.RamSlots,
            request.MaxMemoryGb,
            request.MaxDimmSizeGb,
            request.SataPorts,
            request.FanConnectors,
            request.EpsConnectors,
            request.WidthMm,
            request.HeightMm,
            request.DdrGeneration,
            request.RamFormFactor,
            request.FormFactor,
            request.WifiEnabled,
            request.BluetoothEnabled);

        foreach (var slot in request.PcieSlots)
        {
            entity.AddPcieSlot(new MotherboardPcie(entity.Id, slot.SlotType, slot.SlotLanes, slot.Generation, slot.SlotCount));
        }

        foreach (var slot in request.M2Slots)
        {
            var slotEntity = new MotherboardM2(
                entity.Id, slot.Key, slot.PcieGeneration, slot.SlotCount, slot.SupportsSata);

            foreach (var formFactor in slot.FormFactors)
            {
                slotEntity.AddFormFactor(new MotherboardM2FormFactor(slotEntity.Id, formFactor));
            }
            
            entity.AddM2Slot(slotEntity);
        }

        foreach (var port in request.UsbPorts)
        {
            entity.AddUsbPort(new MotherboardUsb(entity.Id, port.UsbVersion, port.UsbType, port.PortCount));
        }
        
        context.Motherboards.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<MotherboardDto>(entity);
    }
}
