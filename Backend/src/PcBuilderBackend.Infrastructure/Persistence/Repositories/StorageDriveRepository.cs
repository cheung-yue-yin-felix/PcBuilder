using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.StorageDrives;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class StorageDriveRepository(PcBuilderDbContext db) : IStorageDriveRepository
{
    public Task<StorageDrive?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.StorageDrives.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<StorageDrive>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.StorageDrives.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(StorageDrive storageDrive) => db.StorageDrives.Add(storageDrive);
}
