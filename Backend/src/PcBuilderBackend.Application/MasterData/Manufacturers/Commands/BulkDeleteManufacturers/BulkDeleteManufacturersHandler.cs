using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

public class BulkDeleteManufacturersHandler(IApplicationDbContext context) : IRequestHandler<BulkDeleteManufacturersCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteManufacturersCommand request, CancellationToken cancellationToken)
    {
        var ids = request.ManufacturerIds.Distinct().ToList();
        var manufacturers = context.Manufacturers.Where(m => ids.Contains(m.Id) && m.IsActive).ToList();

        if (manufacturers.Count != ids.Count) return false;

        foreach (var manufacturer in manufacturers)
        {
            manufacturer.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}