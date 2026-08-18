using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.DeleteChassis;

public class DeleteChassisHandler(
    IApplicationDbContext context,
    ILogger<DeleteChassisHandler> logger)
    : IRequestHandler<DeleteChassisCommand, bool>
{
    public async Task<bool> Handle(DeleteChassisCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Chassis
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Chassis, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Chassis, entity.Id);
        return true;
    }
}
