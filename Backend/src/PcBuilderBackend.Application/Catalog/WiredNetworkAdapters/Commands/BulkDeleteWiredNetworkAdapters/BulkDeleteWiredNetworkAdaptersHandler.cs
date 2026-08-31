using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkDeleteWiredNetworkAdapters;

public class BulkDeleteWiredNetworkAdaptersHandler(
    IWiredNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeleteWiredNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkDeleteWiredNetworkAdaptersCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteWiredNetworkAdaptersCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var entities = await adapters.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.WiredNetworkAdapter, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.WiredNetworkAdapter);
        return true;
    }
}
