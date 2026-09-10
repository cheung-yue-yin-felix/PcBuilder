using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.MasterData.Manufacturers;

public interface IManufacturerReadStore
{
    Task<List<ManufacturerDto>> ListByProductTypeAsync(
        ProductType productType,
        CancellationToken cancellationToken);
}
