using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class TestRepository<T>(TestApplicationDbContext db) : IRepository<T>
    where T : BaseEntity
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<T>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken) =>
        db.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(T entity) => db.Set<T>().Add(entity);
}
