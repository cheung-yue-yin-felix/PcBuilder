using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Psus;

public interface IPsuRepository
{
    Task<Psu?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Psu?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Psu>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(Psu psu);
    void DeleteCable(PsuCable cable);
}
