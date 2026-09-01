using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Build;

public interface IPcBuildReadStore
{
    Task<PcBuildDto?> GetByIdAsync(Guid id);
    Task<PagedResult<PcBuildListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
}
