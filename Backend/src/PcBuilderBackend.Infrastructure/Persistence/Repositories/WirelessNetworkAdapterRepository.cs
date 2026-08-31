using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class WirelessNetworkAdapterRepository(PcBuilderDbContext db) : IWirelessNetworkAdapterRepository
{
    public Task<WirelessNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.WirelessNetworkAdapters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<WirelessNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.WirelessNetworkAdapters.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(WirelessNetworkAdapter adapter) => db.WirelessNetworkAdapters.Add(adapter);
}
