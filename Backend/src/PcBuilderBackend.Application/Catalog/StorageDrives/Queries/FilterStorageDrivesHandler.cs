using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public class FilterStorageDrivesHandler(IStorageDriveReadStore store)
    : IRequestHandler<FilterStorageDrivesQuery, PagedResult<StorageDriveDto>>
{
    public Task<PagedResult<StorageDriveDto>> Handle(
        FilterStorageDrivesQuery query,
        CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
