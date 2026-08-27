using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers;

public interface ICpuCoolerRepository
{
    Task<CpuCooler?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CpuCooler?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<CpuCooler>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(CpuCooler cpuCooler);
    void DeleteCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket);
}