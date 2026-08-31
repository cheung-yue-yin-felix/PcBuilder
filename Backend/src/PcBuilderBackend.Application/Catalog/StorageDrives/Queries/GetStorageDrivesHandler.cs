using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public class GetStorageDrivesHandler(IStorageDriveReadStore store)
    : IRequestHandler<GetStorageDrivesQuery, PagedResult<StorageDriveDto>>
{
    public Task<PagedResult<StorageDriveDto>> Handle(
        GetStorageDrivesQuery query,
        CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
