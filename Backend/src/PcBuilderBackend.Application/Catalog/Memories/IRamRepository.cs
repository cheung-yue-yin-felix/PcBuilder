using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Memories;

public interface IRamRepository
{
    Task<Ram?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Ram?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ram>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(Ram ram);
}