using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.DeletePsu;

public class DeletePsuHandler(
    IApplicationDbContext context,
    ILogger<DeletePsuHandler> logger)
    : IRequestHandler<DeletePsuCommand, bool>
{
    public async Task<bool> Handle(DeletePsuCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Psus
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Psu, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Psu, entity.Id);
        return true;
    }
}
