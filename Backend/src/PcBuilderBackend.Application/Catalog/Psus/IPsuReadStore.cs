using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus;

public interface IPsuReadStore
{
    Task<PsuDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<PsuListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<PsuListItemDto>> FilterAsync(
        PagedRequest<PsuFilter> request,
        CancellationToken cancellationToken);
    Task<List<PsuCableDto>> ListCablesAsync(Guid psuId, CancellationToken cancellationToken);
}
