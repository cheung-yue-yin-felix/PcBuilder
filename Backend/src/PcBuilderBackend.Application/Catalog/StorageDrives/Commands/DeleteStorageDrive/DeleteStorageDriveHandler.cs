using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.DeleteStorageDrive;

public class DeleteStorageDriveHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    ILogger<DeleteStorageDriveHandler> logger)
    : IRequestHandler<DeleteStorageDriveCommand, bool>
{
    public async Task<bool> Handle(DeleteStorageDriveCommand request, CancellationToken cancellationToken)
    {
        var entity = await storageDrives.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.StorageDrive, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.StorageDrive, entity.Id);
        return true;
    }
}
