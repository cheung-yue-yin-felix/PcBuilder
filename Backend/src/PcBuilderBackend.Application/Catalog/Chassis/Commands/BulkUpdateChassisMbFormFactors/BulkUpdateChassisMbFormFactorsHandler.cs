using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisMbFormFactors;

public class BulkUpdateChassisMbFormFactorsHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    ILogger<BulkUpdateChassisMbFormFactorsHandler> logger)
    : IRequestHandler<BulkUpdateChassisMbFormFactorsCommand, List<MbFormFactor>?>
{
    public async Task<List<MbFormFactor>?> Handle(
        BulkUpdateChassisMbFormFactorsCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await chassis.GetWithChildrenAsync(request.ChassisId, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var requested = request.MbFormFactors.Distinct().ToHashSet();
        var existingByKey = entity.MbFormFactors.ToDictionary(x => x.MbFormFactor);

        foreach (var formFactor in requested)
        {
            if (!existingByKey.ContainsKey(formFactor))
            {
                entity.AddMbFormFactor(new ChassisMbFormFactor(request.ChassisId, formFactor));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !requested.Contains(x.MbFormFactor)))
        {
            entity.RemoveMbFormFactor(existing);
            chassis.DeleteMbFormFactor(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisMbFormFactorsUpdated(logger, entity.Id);

        return
        [
            .. entity.MbFormFactors
                .Where(x => x.IsActive)
                .Select(x => x.MbFormFactor)
        ];
    }
}
