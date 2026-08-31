using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans;

public interface IChassisFanReadStore
{
    Task<ChassisFanDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<ChassisFanDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<ChassisFanDto>> FilterAsync(
        PagedRequest<ChassisFanFilter> request,
        CancellationToken cancellationToken);
}
