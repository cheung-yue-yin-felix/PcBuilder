using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives;

public interface IStorageDriveRepository
{
    Task<StorageDrive?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<StorageDrive>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);
    void Add(StorageDrive storageDrive);
}
