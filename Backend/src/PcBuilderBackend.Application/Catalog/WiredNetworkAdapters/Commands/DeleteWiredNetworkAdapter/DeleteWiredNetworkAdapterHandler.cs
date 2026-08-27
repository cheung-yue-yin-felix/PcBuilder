using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.DeleteWiredNetworkAdapter;

public class DeleteWiredNetworkAdapterHandler(
    IApplicationDbContext context,
    ILogger<DeleteWiredNetworkAdapterHandler> logger)
    : IRequestHandler<DeleteWiredNetworkAdapterCommand, bool>
{
    public async Task<bool> Handle(DeleteWiredNetworkAdapterCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.WiredNetworkAdapters
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WiredNetworkAdapter, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.WiredNetworkAdapter, entity.Id);
        return true;
    }
}
