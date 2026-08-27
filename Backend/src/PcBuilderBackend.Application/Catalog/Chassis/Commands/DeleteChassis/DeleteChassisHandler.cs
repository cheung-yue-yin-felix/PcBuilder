using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.DeleteChassis;

public class DeleteChassisHandler(
    IChassisRepository chassis,
    IUnitOfWork unitOfWork,
    ILogger<DeleteChassisHandler> logger)
    : IRequestHandler<DeleteChassisCommand, bool>
{
    public async Task<bool> Handle(DeleteChassisCommand request, CancellationToken cancellationToken)
    {
        var entity = await chassis.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Chassis, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Chassis, entity.Id);
        return true;
    }
}
