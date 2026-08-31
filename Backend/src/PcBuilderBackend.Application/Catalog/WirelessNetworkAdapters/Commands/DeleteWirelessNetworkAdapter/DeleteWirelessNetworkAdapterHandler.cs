using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.DeleteWirelessNetworkAdapter;

public class DeleteWirelessNetworkAdapterHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    ILogger<DeleteWirelessNetworkAdapterHandler> logger)
    : IRequestHandler<DeleteWirelessNetworkAdapterCommand, bool>
{
    public async Task<bool> Handle(DeleteWirelessNetworkAdapterCommand request, CancellationToken cancellationToken)
    {
        var entity = await adapters.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WirelessNetworkAdapter, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.WirelessNetworkAdapter, entity.Id);
        return true;
    }
}
