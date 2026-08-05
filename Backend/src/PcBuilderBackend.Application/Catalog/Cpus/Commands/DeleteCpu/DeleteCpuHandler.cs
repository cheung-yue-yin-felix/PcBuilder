using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;

public class DeleteCpuHandler(IApplicationDbContext context, ILogger<DeleteCpuHandler> logger): IRequestHandler<DeleteCpuCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = context.Cpus.FirstOrDefault(c => c.Id == request.Id && c.IsActive);

        if (entity == null) 
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, request.Id);
            return false;
        }

        entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Cpu, entity.Id);
        
        return true;
    }
}
