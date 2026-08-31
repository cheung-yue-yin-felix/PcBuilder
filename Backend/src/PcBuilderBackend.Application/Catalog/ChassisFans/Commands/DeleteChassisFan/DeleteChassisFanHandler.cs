using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.DeleteChassisFan;

public class DeleteChassisFanHandler(
    IChassisFanRepository chassisFans,
    IUnitOfWork unitOfWork,
    ILogger<DeleteChassisFanHandler> logger)
    : IRequestHandler<DeleteChassisFanCommand, bool>
{
    public async Task<bool> Handle(DeleteChassisFanCommand request, CancellationToken cancellationToken)
    {
        var entity = await chassisFans.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.ChassisFan, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.ChassisFan, entity.Id);
        return true;
    }
}
