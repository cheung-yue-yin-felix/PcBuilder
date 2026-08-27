using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.DeleteChassisFan;

public class DeleteChassisFanHandler(
    IApplicationDbContext context,
    ILogger<DeleteChassisFanHandler> logger)
    : IRequestHandler<DeleteChassisFanCommand, bool>
{
    public async Task<bool> Handle(DeleteChassisFanCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ChassisFans
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.ChassisFan, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.ChassisFan, entity.Id);
        return true;
    }
}
