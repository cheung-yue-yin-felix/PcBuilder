using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus;

public interface ICpuReadStore
{
    Task<CpuDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<CpuListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);

    Task<PagedResult<CpuListItemDto>> FilterAsync(
        PagedRequest<CpuFilter> request,
        CancellationToken cancellationToken);

    Task<List<CpuRamCompatDto>> ListRamCompatsAsync(Guid cpuId, CancellationToken cancellationToken);

    Task<List<CpuSupportChipsetDto>> ListSupportChipsetsAsync(Guid cpuId, CancellationToken cancellationToken);
}
