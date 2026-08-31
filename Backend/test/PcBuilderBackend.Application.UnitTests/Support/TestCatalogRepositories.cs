using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.ChassisFans;
using PcBuilderBackend.Application.Catalog.StorageDrives;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class TestChassisFanRepository(TestApplicationDbContext db) : IChassisFanRepository
{
    public Task<ChassisFan?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.ChassisFans.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ChassisFan>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.ChassisFans.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(ChassisFan chassisFan) => db.ChassisFans.Add(chassisFan);
}

public sealed class TestStorageDriveRepository(TestApplicationDbContext db) : IStorageDriveRepository
{
    public Task<StorageDrive?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.StorageDrives.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<StorageDrive>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.StorageDrives.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(StorageDrive storageDrive) => db.StorageDrives.Add(storageDrive);
}

public sealed class TestWiredNetworkAdapterRepository(TestApplicationDbContext db) : IWiredNetworkAdapterRepository
{
    public Task<WiredNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.WiredNetworkAdapters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<WiredNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.WiredNetworkAdapters.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(WiredNetworkAdapter adapter) => db.WiredNetworkAdapters.Add(adapter);
}

public sealed class TestWirelessNetworkAdapterRepository(TestApplicationDbContext db)
    : IWirelessNetworkAdapterRepository
{
    public Task<WirelessNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.WirelessNetworkAdapters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<WirelessNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.WirelessNetworkAdapters.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(WirelessNetworkAdapter adapter) => db.WirelessNetworkAdapters.Add(adapter);
}
