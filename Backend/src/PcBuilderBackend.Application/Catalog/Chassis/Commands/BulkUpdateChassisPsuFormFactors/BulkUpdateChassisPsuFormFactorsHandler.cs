using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPsuFormFactors;

public class BulkUpdateChassisPsuFormFactorsHandler(
    IApplicationDbContext context,
    ILogger<BulkUpdateChassisPsuFormFactorsHandler> logger)
    : IRequestHandler<BulkUpdateChassisPsuFormFactorsCommand, List<PsuFormFactor>?>
{
    public async Task<List<PsuFormFactor>?> Handle(
        BulkUpdateChassisPsuFormFactorsCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.PsuFormFactors)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var requested = request.PsuFormFactors.Distinct().ToHashSet();
        var existingByKey = chassis.PsuFormFactors.ToDictionary(x => x.PsuFormFactor);

        foreach (var formFactor in requested)
        {
            if (!existingByKey.ContainsKey(formFactor))
            {
                chassis.AddPsuFormFactor(new ChassisPsuFormFactor(request.ChassisId, formFactor));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !requested.Contains(x.PsuFormFactor)))
        {
            chassis.RemovePsuFormFactor(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisPsuFormFactorsUpdated(logger, chassis.Id);

        return [.. chassis.PsuFormFactors
            .Where(x => x.IsActive)
            .Select(x => x.PsuFormFactor)];
    }
}
