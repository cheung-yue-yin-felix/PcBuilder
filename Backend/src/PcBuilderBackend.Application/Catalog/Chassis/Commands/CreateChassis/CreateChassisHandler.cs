using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

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
            new ChassisSpecs
            {
                LengthMm = request.LengthMm,
                WidthMm = request.WidthMm,
                HeightMm = request.HeightMm,
                MotherboardMaxWidthMm = request.MotherboardMaxWidthMm,
                MotherboardMaxHeightMm = request.MotherboardMaxHeightMm,
                MaxCpuCoolerHeightMm = request.MaxCpuCoolerHeightMm,
                MaxGraphicsCardLengthMm = request.MaxGraphicsCardLengthMm,
                MaxPsuLengthMm = request.MaxPsuLengthMm
            });

        ChassisChildCollections.Apply(
            entity,
            request.DriveBays,
            request.FanMounts,
            request.PcieSlots,
            request.Radiators,
            request.MbFormFactors,
            request.PsuFormFactors);

        chassis.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.Chassis, entity.Id);

        return mapper.Map<ChassisDto>(entity);
    }
}
