using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkDeleteWirelessNetworkAdapters;

public class BulkDeleteWirelessNetworkAdaptersHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    ILogger<BulkDeleteWirelessNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkDeleteWirelessNetworkAdaptersCommand, bool>
{
    public async Task<bool> Handle(
        BulkDeleteWirelessNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var entities = await adapters.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.WirelessNetworkAdapter, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.WirelessNetworkAdapter);
        return true;
    }
}
