using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class PsuRepository(PcBuilderDbContext db) : IPsuRepository
{
    public Task<Psu?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Psus.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Psu?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken) =>
        db.Psus
            .Include(x => x.Cables)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Psu>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.Psus.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(Psu psu) => db.Psus.Add(psu);

    public void DeleteCable(PsuCable cable) => db.PsuCables.Remove(cable);
}
