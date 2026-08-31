using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.ChassisFans;

public interface IChassisFanRepository
{
    Task<ChassisFan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ChassisFan>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);
    void Add(ChassisFan chassisFan);
}
