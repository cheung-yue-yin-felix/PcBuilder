using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.DeleteCpuSeries;

public class DeleteCpuSeriesHandler(IApplicationDbContext context) : IRequestHandler<DeleteCpuSeriesCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var cpuSeries = await context.CpuSeries.FirstOrDefaultAsync(x => x.Id == request.CpuSeriesId && x.IsActive, cancellationToken);
        if (cpuSeries is null) return false;
        cpuSeries.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}