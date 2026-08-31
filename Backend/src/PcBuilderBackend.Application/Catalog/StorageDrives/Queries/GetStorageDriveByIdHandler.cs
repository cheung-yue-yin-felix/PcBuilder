using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public class GetStorageDriveByIdHandler(IStorageDriveReadStore store)
    : IRequestHandler<GetStorageDriveByIdQuery, StorageDriveDto?>
{
    public Task<StorageDriveDto?> Handle(GetStorageDriveByIdQuery request, CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
