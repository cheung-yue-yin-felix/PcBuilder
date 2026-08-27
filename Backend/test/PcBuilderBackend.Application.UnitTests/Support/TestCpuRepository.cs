using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class TestCpuRepository(TestApplicationDbContext db) : ICpuRepository
{
    public Task<Cpu?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Cpus.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Cpu?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken) =>
        db.Cpus
            .Include(x => x.RamCompats)
            .Include(x => x.SupportedChipsets)
                .ThenInclude(x => x.Chipset)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Cpu>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.Cpus.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public void Add(Cpu cpu) => db.Cpus.Add(cpu);

    public void DeleteRamCompat(CpuRamCompat ramCompat) => db.CpuRamCompats.Remove(ramCompat);

    public void DeleteSupportedChipset(CpuSupportChipset supportChipset) =>
        db.CpuSupportChipsets.Remove(supportChipset);
}

public sealed class TestUnitOfWork(TestApplicationDbContext db) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        db.SaveChangesAsync(cancellationToken);
}
