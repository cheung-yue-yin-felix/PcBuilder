using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkDeleteChassisFans;

public class BulkDeleteChassisFansHandler(
    IApplicationDbContext context,
    ILogger<BulkDeleteChassisFansHandler> logger)
    : IRequestHandler<BulkDeleteChassisFansCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteChassisFansCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await context.ChassisFans
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.ChassisFan, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.ChassisFan);
        return true;
    }
}
