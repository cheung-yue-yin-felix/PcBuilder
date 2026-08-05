using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkDeleteMotherboards;

public class BulkDeleteMotherboardsHandler(IApplicationDbContext context) : IRequestHandler<BulkDeleteMotherboardsCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteMotherboardsCommand request, CancellationToken cancellationToken)
    {
        var ids = request.MotherboardIds.Distinct().ToList();
        var entities = await context.Motherboards
            .Where(m => ids.Contains(m.Id) && m.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            return false; // Some motherboards were not found or are already inactive
        }

        foreach (var entity in entities)
        {
            entity.Deactivate(); // Soft delete
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}