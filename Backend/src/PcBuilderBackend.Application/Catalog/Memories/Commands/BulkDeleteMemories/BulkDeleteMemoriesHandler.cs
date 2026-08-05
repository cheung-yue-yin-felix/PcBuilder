using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkDeleteMemories;

public class BulkDeleteMemoriesHandler(IApplicationDbContext context) : IRequestHandler<BulkDeleteMemoriesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteMemoriesCommand request, CancellationToken cancellationToken)
    {
        var ids = request.MemoryIds.Distinct().ToList();
        var entities = await context.Rams.Where(x => ids.Contains(x.Id) && x.IsActive).ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
            return false;

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}