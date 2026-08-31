using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.ChassisFans;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class ChassisFanRepository(PcBuilderDbContext db) : IChassisFanRepository
{
    public Task<ChassisFan?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.ChassisFans.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ChassisFan>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.ChassisFans.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(ChassisFan chassisFan) => db.ChassisFans.Add(chassisFan);
}
