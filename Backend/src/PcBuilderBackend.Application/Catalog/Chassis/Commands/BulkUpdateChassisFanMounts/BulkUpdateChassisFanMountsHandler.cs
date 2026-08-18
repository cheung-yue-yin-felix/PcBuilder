using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisFanMounts;

public class BulkUpdateChassisFanMountsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateChassisFanMountsHandler> logger)
    : IRequestHandler<BulkUpdateChassisFanMountsCommand, List<ChassisFanMountDto>?>
{
    public async Task<List<ChassisFanMountDto>?> Handle(
        BulkUpdateChassisFanMountsCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.FanMounts)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var existingByKey = chassis.FanMounts.ToDictionary(x => x.Location);
        var touchedKeys = new HashSet<FanMountLocation>();

        foreach (var mount in request.FanMounts)
        {
            touchedKeys.Add(mount.Location);

            if (existingByKey.TryGetValue(mount.Location, out var existing))
            {
                existing.UpdateSpecs(request.ChassisId, mount.Location, mount.SingleDiameterOnly);
                SyncOptions(existing, mount.Options);
            }
            else
            {
                var mountEntity = new ChassisFanMount(
                    request.ChassisId,
                    mount.Location,
                    mount.SingleDiameterOnly);

                foreach (var option in mount.Options.DistinctBy(x => x.Diameter))
                {
                    mountEntity.AddOption(new ChassisFanMountOption(
                        mountEntity.Id,
                        option.Diameter,
                        option.SlotCount));
                }

                chassis.AddFanMount(mountEntity);
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.Location)))
        {
            foreach (var option in existing.Options.ToList())
            {
                existing.RemoveOption(option);
                context.ChassisFanMountOptions.Remove(option);
            }

            chassis.RemoveFanMount(existing);
            context.ChassisFanMounts.Remove(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisFanMountsUpdated(logger, chassis.Id);

        return [.. chassis.FanMounts
            .Where(x => x.IsActive)
            .Select(mapper.Map<ChassisFanMountDto>)];
    }

    private void SyncOptions(ChassisFanMount existing, IEnumerable<ChassisFanMountOptionDto> options)
    {
        var requestedByKey = options
            .DistinctBy(x => x.Diameter)
            .ToDictionary(x => x.Diameter);

        var existingByKey = existing.Options.ToDictionary(x => x.Diameter);

        foreach (var (diameter, option) in requestedByKey)
        {
            if (existingByKey.TryGetValue(diameter, out var current))
            {
                current.UpdateSpecs(existing.Id, option.Diameter, option.SlotCount);
            }
            else
            {
                existing.AddOption(new ChassisFanMountOption(existing.Id, option.Diameter, option.SlotCount));
            }
        }

        foreach (var optionEntity in existingByKey.Values.Where(x => !requestedByKey.ContainsKey(x.Diameter)))
        {
            existing.RemoveOption(optionEntity);
            context.ChassisFanMountOptions.Remove(optionEntity);
        }
    }
}
