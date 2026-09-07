using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

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
            var entity = CreateChassis(item);
            chassis.Add(entity);
            result.Add(mapper.Map<ChassisDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Chassis);
        return result;
    }

    private static Domain.Entities.Chassis CreateChassis(CreateChassisItem item)
    {
        var entity = new Domain.Entities.Chassis(
            item.Name,
            item.ManufacturerId,
            new ChassisSpecs
            {
                LengthMm = item.LengthMm,
                WidthMm = item.WidthMm,
                HeightMm = item.HeightMm,
                MotherboardMaxWidthMm = item.MotherboardMaxWidthMm,
                MotherboardMaxHeightMm = item.MotherboardMaxHeightMm,
                MaxCpuCoolerHeightMm = item.MaxCpuCoolerHeightMm,
                MaxGraphicsCardLengthMm = item.MaxGraphicsCardLengthMm,
                MaxPsuLengthMm = item.MaxPsuLengthMm
            });

        ChassisChildCollections.Apply(
            entity,
            item.DriveBays,
            item.FanMounts,
            item.PcieSlots,
            item.Radiators,
            item.MbFormFactors,
            item.PsuFormFactors);

        return entity;
    }
}
