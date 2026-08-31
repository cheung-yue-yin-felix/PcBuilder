using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;

public interface IWirelessNetworkAdapterReadStore
{
    Task<WirelessNetworkAdapterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<WirelessNetworkAdapterDto>> ListAsync(
        PagedRequest request,
        CancellationToken cancellationToken);
    Task<PagedResult<WirelessNetworkAdapterDto>> FilterAsync(
        PagedRequest<WirelessNetworkAdapterFilter> request,
        CancellationToken cancellationToken);
}
