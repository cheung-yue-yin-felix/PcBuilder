using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.DeleteMotherboard;

public class DeleteMotherboardHandler(IApplicationDbContext context) : IRequestHandler<DeleteMotherboardCommand, bool>
{
    public async Task<bool> Handle(DeleteMotherboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Motherboards
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
