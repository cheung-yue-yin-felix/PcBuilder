using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.DeleteGpu;

public class DeleteGpuHandler(IApplicationDbContext context, ILogger<DeleteGpuHandler> logger) : IRequestHandler<DeleteGpuCommand, bool>
{
    public async Task<bool> Handle(DeleteGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = await context.Gpus.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive, cancellationToken);
        if (gpu is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Gpu, request.Id);
            return false;
        }
        
        gpu.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.Gpu, request.Id);
        return true;
    }
}