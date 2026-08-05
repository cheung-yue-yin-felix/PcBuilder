using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkDeleteGpuSeries;

public class BulkDeleteGpuSeriesHandler(IApplicationDbContext context): IRequestHandler<BulkDeleteGpuSeriesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var entity = await context.GpuSeries.FirstOrDefaultAsync(g => g.Id == id && g.IsActive, cancellationToken);
            
            if (entity is null) return false;
            
            entity.Deactivate();
        }
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}