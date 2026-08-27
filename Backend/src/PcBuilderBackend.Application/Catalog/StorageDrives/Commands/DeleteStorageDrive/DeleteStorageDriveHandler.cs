using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.DeleteStorageDrive;

public class DeleteStorageDriveHandler(
    IApplicationDbContext context,
    ILogger<DeleteStorageDriveHandler> logger)
    : IRequestHandler<DeleteStorageDriveCommand, bool>
{
    public async Task<bool> Handle(DeleteStorageDriveCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.StorageDrives
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.StorageDrive, request.Id);
            return false;
        }

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.StorageDrive, entity.Id);
        return true;
    }
}
