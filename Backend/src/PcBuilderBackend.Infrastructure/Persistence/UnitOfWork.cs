using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Infrastructure.Persistence;

public sealed class UnitOfWork(IApplicationDbContext db) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        db.SaveChangesAsync(cancellationToken);
}
