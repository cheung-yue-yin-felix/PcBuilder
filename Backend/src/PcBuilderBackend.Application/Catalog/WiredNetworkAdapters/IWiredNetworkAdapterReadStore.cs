using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;

public interface IWiredNetworkAdapterReadStore
{
    Task<WiredNetworkAdapterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<WiredNetworkAdapterDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<WiredNetworkAdapterDto>> FilterAsync(
        PagedRequest<WiredNetworkAdapterFilter> request,
        CancellationToken cancellationToken);
}
