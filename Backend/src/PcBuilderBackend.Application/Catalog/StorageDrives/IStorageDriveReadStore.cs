using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives;

public interface IStorageDriveReadStore
{
    Task<StorageDriveDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<StorageDriveDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<StorageDriveDto>> FilterAsync(
        PagedRequest<StorageDriveFilter> request,
        CancellationToken cancellationToken);
}
