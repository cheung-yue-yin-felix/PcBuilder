using PcBuilderBackend.Application.Catalog.CpuCoolers;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public sealed class CpuCoolerRepository(PcBuilderDbContext db) : ICpuCoolerRepository
{
    public Task<CpuCooler?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.CpuCoolers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    public Task<CpuCooler?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken) =>
        db.CpuCoolers
            .Include(x => x.CpuCoolerSockets)
                .ThenInclude(x => x.Socket)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    public async Task<IReadOnlyList<CpuCooler>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await db.CpuCoolers.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    
    public void Add(CpuCooler cpuCooler) => db.CpuCoolers.Add(cpuCooler);

    public void DeleteCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket) =>
        db.CpuCoolerSockets.Remove(cpuCoolerSocket);
}