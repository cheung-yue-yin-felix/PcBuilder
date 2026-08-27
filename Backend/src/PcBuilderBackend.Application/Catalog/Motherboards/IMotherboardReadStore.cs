using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards;

public interface IMotherboardReadStore
{
    Task<MotherboardDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<MotherboardListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken);

    Task<PagedResult<MotherboardListItemDto>> FilterAsync(PagedRequest<MotherboardFilter> request,
        CancellationToken cancellationToken);
    
    Task<List<MotherboardM2Dto>> ListM2SlotsAsync(Guid motherboardId, CancellationToken cancellationToken);
    Task<List<MotherboardPcieDto>> ListPcieSlotsAsync(Guid motherboardId, CancellationToken cancellationToken);
    Task<List<MotherboardUsbDto>> ListUsbPortsAsync(Guid motherboardId, CancellationToken cancellationToken);
}