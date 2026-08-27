using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.DeleteWirelessNetworkAdapter;

public class DeleteWirelessNetworkAdapterHandler(
    IApplicationDbContext context,
    ILogger<DeleteWirelessNetworkAdapterHandler> logger)
    : IRequestHandler<DeleteWirelessNetworkAdapterCommand, bool>
{
    public async Task<bool> Handle(DeleteWirelessNetworkAdapterCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.WirelessNetworkAdapters
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WirelessNetworkAdapter, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.WirelessNetworkAdapter, entity.Id);
        return true;
    }
}
