using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.DeleteCpuCooler;

public class DeleteCpuCoolerHandler(
    ICpuCoolerRepository cpuCoolers,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCpuCoolerHandler> logger)
    : IRequestHandler<DeleteCpuCoolerCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuCoolerCommand request, CancellationToken cancellationToken)
    {
        var entity = await cpuCoolers.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.CpuCooler, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.CpuCooler, entity.Id);

        return true;
    }
}
