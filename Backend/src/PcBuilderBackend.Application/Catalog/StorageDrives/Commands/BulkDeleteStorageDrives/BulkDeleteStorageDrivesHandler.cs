using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkDeleteStorageDrives;

public class BulkDeleteStorageDrivesHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeleteStorageDrivesHandler> logger)
    : IRequestHandler<BulkDeleteStorageDrivesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteStorageDrivesCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var entities = await storageDrives.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.StorageDrive, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.StorageDrive);
        return true;
    }
}
