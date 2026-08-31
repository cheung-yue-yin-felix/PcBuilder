using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.DeleteWiredNetworkAdapter;

public class DeleteWiredNetworkAdapterHandler(
    IWiredNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    ILogger<DeleteWiredNetworkAdapterHandler> logger)
    : IRequestHandler<DeleteWiredNetworkAdapterCommand, bool>
{
    public async Task<bool> Handle(DeleteWiredNetworkAdapterCommand request, CancellationToken cancellationToken)
    {
        var entity = await adapters.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WiredNetworkAdapter, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.WiredNetworkAdapter, entity.Id);
        return true;
    }
}
