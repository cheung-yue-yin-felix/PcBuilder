using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.DeleteMemory;

public class DeleteMemoryHandler(IApplicationDbContext context) : IRequestHandler<DeleteMemoryCommand, bool>
{
    public async Task<bool> Handle(DeleteMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Rams.FirstOrDefaultAsync(r => r.Id == request.Id && r.IsActive, cancellationToken);
        if (entity == null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
