using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.DeleteGraphicsCard;

public class DeleteGraphicsCardHandler(
    IApplicationDbContext context,
    ILogger<DeleteGraphicsCardHandler> logger)
    : IRequestHandler<DeleteGraphicsCardCommand, bool>
{
    public async Task<bool> Handle(DeleteGraphicsCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GraphicsCards
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.GraphicsCard, entity.Id);
        return true;
    }
}
