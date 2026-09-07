using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPsuFormFactors;

public class BulkUpdateChassisPsuFormFactorsHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    ILogger<BulkUpdateChassisPsuFormFactorsHandler> logger)
    : IRequestHandler<BulkUpdateChassisPsuFormFactorsCommand, List<PsuFormFactor>?>
{
    public async Task<List<PsuFormFactor>?> Handle(
        BulkUpdateChassisPsuFormFactorsCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await chassis.GetWithChildrenAsync(request.ChassisId, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFound(logger, EntityLog.Chassis, request.ChassisId);
            return null;
        }

        var requested = request.PsuFormFactors.Distinct().ToHashSet();
        var existingByKey = entity.PsuFormFactors.ToDictionary(x => x.PsuFormFactor);

        foreach (var formFactor in requested.Where(formFactor => !existingByKey.ContainsKey(formFactor)))
        {
            entity.AddPsuFormFactor(new ChassisPsuFormFactor(request.ChassisId, formFactor));
        }

        foreach (var existing in existingByKey.Values.Where(x => !requested.Contains(x.PsuFormFactor)))
        {
            entity.RemovePsuFormFactor(existing);
            chassis.DeletePsuFormFactor(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.ChassisPsuFormFactorsUpdated(logger, entity.Id);

        return [.. entity.PsuFormFactors
            .Where(x => x.IsActive)
            .Select(x => x.PsuFormFactor)];
    }
}
