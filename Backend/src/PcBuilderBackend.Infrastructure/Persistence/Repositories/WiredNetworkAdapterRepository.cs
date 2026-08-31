using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class WiredNetworkAdapterRepository(PcBuilderDbContext db) : IWiredNetworkAdapterRepository
{
    public Task<WiredNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.WiredNetworkAdapters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<WiredNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.WiredNetworkAdapters.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(WiredNetworkAdapter adapter) => db.WiredNetworkAdapters.Add(adapter);
}
