using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;

public interface IWiredNetworkAdapterRepository
{
    Task<WiredNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<WiredNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);
    void Add(WiredNetworkAdapter adapter);
}
