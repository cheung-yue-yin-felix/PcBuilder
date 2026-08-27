using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;

public class DeleteCpuHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCpuHandler> logger)
    : IRequestHandler<DeleteCpuCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = await cpus.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, request.Id);
            return false;
        }

        entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Cpu, entity.Id);

        return true;
    }
}
