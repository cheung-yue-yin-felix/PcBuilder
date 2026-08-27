using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.DeleteGraphicsCard;

public class DeleteGraphicsCardHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
    ILogger<DeleteGraphicsCardHandler> logger)
    : IRequestHandler<DeleteGraphicsCardCommand, bool>
{
    public async Task<bool> Handle(DeleteGraphicsCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await graphicsCards.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.GraphicsCard, entity.Id);
        return true;
    }
}
