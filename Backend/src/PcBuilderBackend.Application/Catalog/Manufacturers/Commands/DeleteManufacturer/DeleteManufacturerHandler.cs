using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerHandler(IApplicationDbContext context): IRequestHandler<DeleteManufacturerCommand, bool>
{
    public async Task<bool> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers.FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return false;
        
        entity.IsActive = false;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}