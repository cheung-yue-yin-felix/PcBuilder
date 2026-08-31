using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;

public interface IWirelessNetworkAdapterRepository
{
    Task<WirelessNetworkAdapter?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<WirelessNetworkAdapter>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);
    void Add(WirelessNetworkAdapter adapter);
}
