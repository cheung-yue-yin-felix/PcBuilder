using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public class GetManufacturersByProductTypeHandler(IManufacturerReadStore store)
    : IRequestHandler<GetManufacturersByProductTypeQuery, List<ManufacturerDto>>
{
    public Task<List<ManufacturerDto>> Handle(
        GetManufacturersByProductTypeQuery query,
        CancellationToken cancellationToken) =>
        store.ListByProductTypeAsync(query.ProductType, cancellationToken);
}
