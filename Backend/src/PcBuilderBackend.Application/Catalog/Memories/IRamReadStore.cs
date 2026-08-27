using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories;

public interface IRamReadStore
{
    Task<RamDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<RamDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<RamDto>> FilterAsync(PagedRequest<RamFilter> request, CancellationToken cancellationToken);
}