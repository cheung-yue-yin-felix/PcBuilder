using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.CreateChassis;

public class CreateChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateChassisHandler> logger)
    : IRequestHandler<CreateChassisCommand, ChassisDto>
{
    public async Task<ChassisDto> Handle(CreateChassisCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Chassis(
            request.Name,
            request.ManufacturerId,
            request.LengthMm,
            request.WidthMm,
            request.HeightMm,
            request.MotherboardMaxWidthMm,
            request.MotherboardMaxHeightMm,
            request.MaxCpuCoolerHeightMm,
            request.MaxGraphicsCardLengthMm,
            request.MaxPsuLengthMm);

        ApplyChildCollections(entity, request);

        chassis.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.Chassis, entity.Id);

        return mapper.Map<ChassisDto>(entity);
    }

    internal static void ApplyChildCollections(Domain.Entities.Chassis entity, CreateChassisCommand request)
    {
        foreach (var bay in request.DriveBays)
        {
            entity.AddDriveBay(new ChassisDriveBay(entity.Id, bay.FormFactor, bay.SlotCount));
        }

        foreach (var mount in request.FanMounts)
        {
            var mountEntity = new ChassisFanMount(entity.Id, mount.Location, mount.SingleDiameterOnly);

            foreach (var option in mount.Options)
            {
                mountEntity.AddOption(new ChassisFanMountOption(mountEntity.Id, option.Diameter, option.SlotCount));
            }

            entity.AddFanMount(mountEntity);
        }

        foreach (var slot in request.PcieSlots)
        {
            entity.AddPcieSlot(new ChassisPcieSlot(
                entity.Id,
                slot.LowProfileSlots,
                slot.SlotCount,
                slot.Orientation));
        }

        foreach (var radiator in request.Radiators)
        {
            entity.AddRadiator(new ChassisRadiator(
                entity.Id,
                radiator.Length,
                radiator.Location,
                radiator.RadiatorCount));
        }

        foreach (var formFactor in request.MbFormFactors.Distinct())
        {
            entity.AddMbFormFactor(new ChassisMbFormFactor(entity.Id, formFactor));
        }

        foreach (var formFactor in request.PsuFormFactors.Distinct())
        {
            entity.AddPsuFormFactor(new ChassisPsuFormFactor(entity.Id, formFactor));
        }
    }
}
