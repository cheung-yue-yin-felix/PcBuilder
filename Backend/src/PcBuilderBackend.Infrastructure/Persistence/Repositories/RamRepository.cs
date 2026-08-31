using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public class RamRepository(PcBuilderDbContext db) : IRamRepository
{
    public async Task<Ram?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await db.Rams.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    public async Task<Ram?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken) =>
        await db.Rams
            .Include(x => x.Manufacturer)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    public async Task<IReadOnlyList<Ram>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.Rams.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    
    public void Add(Ram ram) => db.Rams.Add(ram);
}