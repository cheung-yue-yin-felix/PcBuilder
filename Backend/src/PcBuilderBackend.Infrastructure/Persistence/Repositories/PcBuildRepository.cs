using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Build;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public class PcBuildRepository(PcBuilderDbContext context) : IPcBuildRepository
{
    public async Task<PcBuild?> GetByIdAsync(Guid id) =>
        await context.PcBuilds.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<PcBuild?> GetWithChildrenAsync(Guid id) =>
        await context.PcBuilds
            .Include(p => p.User)
            .Include(p => p.ChassisFans)
            .Include(p => p.StorageDevices)
            .Include(p => p.WiredNetworkAdapters)
            .Include(p => p.WirelessNetworkAdapters)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<PcBuild>> GetByIdsAsync(List<Guid> ids) =>
        await context.PcBuilds.Where(p => ids.Contains(p.Id)).ToListAsync();

    public void Add(PcBuild pcBuild) => context.PcBuilds.Add(pcBuild);

    public void AddUser(PcBuildUser pcBuildUser) => context.PcBuildUsers.Add(pcBuildUser);

    public void DeleteUser(PcBuildUser pcBuildUser) => context.PcBuildUsers.Remove(pcBuildUser);

    public void DeletePart(PcBuildPart pcBuildPart) => context.PcBuildParts.Remove(pcBuildPart);
}