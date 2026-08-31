using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkDeletePsus;

public class BulkDeletePsusHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeletePsusHandler> logger)
    : IRequestHandler<BulkDeletePsusCommand, bool>
{
    public async Task<bool> Handle(BulkDeletePsusCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var entities = await psus.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Psu, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Psu);
        return true;
    }
}
