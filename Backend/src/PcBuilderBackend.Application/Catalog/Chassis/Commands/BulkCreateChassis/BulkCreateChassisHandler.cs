using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkCreateChassis;

public class BulkCreateChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkCreateChassisHandler> logger)
    : IRequestHandler<BulkCreateChassisCommand, List<ChassisDto>>
{
    public async Task<List<ChassisDto>> Handle(
        BulkCreateChassisCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<ChassisDto>();

        foreach (var item in request.Items)
        {
            var entity = new Domain.Entities.Chassis(
                item.Name,
                item.ManufacturerId,
                item.LengthMm,
                item.WidthMm,
                item.HeightMm,
                item.MotherboardMaxWidthMm,
                item.MotherboardMaxHeightMm,
                item.MaxCpuCoolerHeightMm,
                item.MaxGraphicsCardLengthMm,
                item.MaxPsuLengthMm);

            foreach (var bay in item.DriveBays)
            {
                entity.AddDriveBay(new ChassisDriveBay(entity.Id, bay.FormFactor, bay.SlotCount));
            }

            foreach (var mount in item.FanMounts)
            {
                var mountEntity = new ChassisFanMount(entity.Id, mount.Location, mount.SingleDiameterOnly);

                foreach (var option in mount.Options)
                {
                    mountEntity.AddOption(new ChassisFanMountOption(
                        mountEntity.Id,
                        option.Diameter,
                        option.SlotCount));
                }

                entity.AddFanMount(mountEntity);
            }

            foreach (var slot in item.PcieSlots)
            {
                entity.AddPcieSlot(new ChassisPcieSlot(
                    entity.Id,
                    slot.LowProfileSlots,
                    slot.SlotCount,
                    slot.Orientation));
            }

            foreach (var radiator in item.Radiators)
            {
                entity.AddRadiator(new ChassisRadiator(
                    entity.Id,
                    radiator.Length,
                    radiator.Location,
                    radiator.RadiatorCount));
            }

            foreach (var formFactor in item.MbFormFactors.Distinct())
            {
                entity.AddMbFormFactor(new ChassisMbFormFactor(entity.Id, formFactor));
            }

            foreach (var formFactor in item.PsuFormFactors.Distinct())
            {
                entity.AddPsuFormFactor(new ChassisPsuFormFactor(entity.Id, formFactor));
            }

            chassis.Add(entity);
            result.Add(mapper.Map<ChassisDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Chassis);
        return result;
    }
}
