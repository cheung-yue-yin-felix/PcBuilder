using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisMbFormFactors;

public class BulkUpdateChassisMbFormFactorsHandler(
    IApplicationDbContext context,
    ILogger<BulkUpdateChassisMbFormFactorsHandler> logger)
    : IRequestHandler<BulkUpdateChassisMbFormFactorsCommand, List<MbFormFactor>?>
{
    public async Task<List<MbFormFactor>?> Handle(
        BulkUpdateChassisMbFormFactorsCommand request,
        CancellationToken cancellationToken)
    {
        var chassis = await context.Chassis
            .Include(x => x.MbFormFactors)
            .FirstOrDefaultAsync(x => x.Id == request.ChassisId && x.IsActive, cancellationToken);

        if (chassis is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var requested = request.MbFormFactors.Distinct().ToHashSet();
        var existingByKey = chassis.MbFormFactors.ToDictionary(x => x.MbFormFactor);

        foreach (var formFactor in requested)
        {
            if (!existingByKey.ContainsKey(formFactor))
            {
                chassis.AddMbFormFactor(new ChassisMbFormFactor(request.ChassisId, formFactor));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !requested.Contains(x.MbFormFactor)))
        {
            chassis.RemoveMbFormFactor(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisMbFormFactorsUpdated(logger, chassis.Id);

        return [.. chassis.MbFormFactors
            .Where(x => x.IsActive)
            .Select(x => x.MbFormFactor)];
    }
}
