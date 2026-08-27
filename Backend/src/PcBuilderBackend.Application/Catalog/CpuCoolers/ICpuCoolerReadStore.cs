using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers;

public interface ICpuCoolerReadStore
{
    Task<CpuCoolerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<CpuCoolerListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);
    Task<PagedResult<CpuCoolerListItemDto>> FilterAsync(PagedRequest<CpuCoolerFilter> request, CancellationToken cancellationToken);
    Task<List<CpuCoolerSocketDto>> ListCpuCoolerSockets(Guid cpuCoolerId, CancellationToken cancellationToken);
}