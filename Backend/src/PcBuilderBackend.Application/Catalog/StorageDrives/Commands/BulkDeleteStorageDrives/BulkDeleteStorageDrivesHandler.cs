using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkDeleteStorageDrives;

public class BulkDeleteStorageDrivesHandler(
    IApplicationDbContext context,
    ILogger<BulkDeleteStorageDrivesHandler> logger)
    : IRequestHandler<BulkDeleteStorageDrivesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteStorageDrivesCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await context.StorageDrives
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.StorageDrive, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.StorageDrive);
        return true;
    }
}
