using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.ImportChassis;

public class ImportChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportChassisHandler> logger)
    : IRequestHandler<ImportChassisCommand, List<ChassisDto>>
{
    public async Task<List<ChassisDto>> Handle(ImportChassisCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParseChassisImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<Domain.Entities.Chassis>();

        foreach (var row in rows)
        {
            var entity = new Domain.Entities.Chassis(
                row.Name,
                row.ManufacturerId,
                row.LengthMm,
                row.WidthMm,
                row.HeightMm,
                row.MotherboardMaxWidthMm,
                row.MotherboardMaxHeightMm,
                row.MaxCpuCoolerHeightMm,
                row.MaxGraphicsCardLengthMm,
                row.MaxPsuLengthMm);

            foreach (var bay in row.DriveBays)
            {
                entity.AddDriveBay(new ChassisDriveBay(entity.Id, bay.FormFactor, bay.SlotCount));
            }

            foreach (var mount in row.FanMounts)
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

            foreach (var slot in row.PcieSlots)
            {
                entity.AddPcieSlot(new ChassisPcieSlot(
                    entity.Id,
                    slot.LowProfileSlots,
                    slot.SlotCount,
                    slot.Orientation));
            }

            foreach (var radiator in row.Radiators)
            {
                entity.AddRadiator(new ChassisRadiator(
                    entity.Id,
                    radiator.Length,
                    radiator.Location,
                    radiator.RadiatorCount));
            }

            foreach (var formFactor in row.MbFormFactors.Select(x => x.MbFormFactor).Distinct())
            {
                entity.AddMbFormFactor(new ChassisMbFormFactor(entity.Id, formFactor));
            }

            foreach (var formFactor in row.PsuFormFactors.Select(x => x.PsuFormFactor).Distinct())
            {
                entity.AddPsuFormFactor(new ChassisPsuFormFactor(entity.Id, formFactor));
            }

            chassis.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.Chassis);

        return [.. result.Select(mapper.Map<ChassisDto>)];
    }
}
