using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassis;

public class BulkUpdateChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateChassisHandler> logger)
    : IRequestHandler<BulkUpdateChassisCommand, List<ChassisDto>?>
{
    public async Task<List<ChassisDto>?> Handle(
        BulkUpdateChassisCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<ChassisDto>();

        foreach (var item in request.Items)
        {
            var entity = await chassis.GetByIdAsync(item.Id, cancellationToken);    

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.Chassis, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(new ChassisSpecs
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

            result.Add(mapper.Map<ChassisDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Chassis);
        return result;
    }
}
