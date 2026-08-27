using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkDeletePsus;

public class BulkDeletePsusHandler(
    IApplicationDbContext context,
    ILogger<BulkDeletePsusHandler> logger)
    : IRequestHandler<BulkDeletePsusCommand, bool>
{
    public async Task<bool> Handle(BulkDeletePsusCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await context.Psus
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Psu, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Psu);
        return true;
    }
}
