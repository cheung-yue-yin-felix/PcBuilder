using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus;

public interface ICpuRepository
{
    Task<Cpu?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Cpu?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Cpu>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(Cpu cpu);
    void DeleteRamCompat(CpuRamCompat ramCompat);
    void DeleteSupportedChipset(CpuSupportChipset supportChipset);
}
