using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkDeleteGraphicsCards;

public class BulkDeleteGraphicsCardsHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeleteGraphicsCardsHandler> logger)
    : IRequestHandler<BulkDeleteGraphicsCardsCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteGraphicsCardsCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var entities = await graphicsCards.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.GraphicsCard, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.GraphicsCard);
        return true;
    }
}
