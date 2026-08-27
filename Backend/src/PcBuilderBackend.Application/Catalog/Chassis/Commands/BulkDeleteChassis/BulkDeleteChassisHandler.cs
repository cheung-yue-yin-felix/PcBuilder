using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkDeleteChassis;

public class BulkDeleteChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeleteChassisHandler> logger)
    : IRequestHandler<BulkDeleteChassisCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteChassisCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await chassis.GetByIdsAsync(ids, cancellationToken);
        
        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Chassis, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Chassis);
        return true;
    }
}
