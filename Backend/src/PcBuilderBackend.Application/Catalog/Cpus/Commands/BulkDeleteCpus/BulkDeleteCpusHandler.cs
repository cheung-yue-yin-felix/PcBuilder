using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkDeleteCpus;

public class BulkDeleteCpusHandler(IApplicationDbContext context, ILogger<BulkDeleteCpusHandler> logger) : IRequestHandler<BulkDeleteCpusCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteCpusCommand request, CancellationToken cancellationToken)
    {
        var ids = request.CpuIds.Distinct().ToList();

        var entities = await context.Cpus
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Cpu, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Cpu);
        
        return true;
    }
}