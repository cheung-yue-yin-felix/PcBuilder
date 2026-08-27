using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardM2SlotsByMotherboardIdHandler(IMotherboardReadStore store)
    : IRequestHandler<GetMotherboardM2SlotsByMotherboardIdQuery, List<MotherboardM2Dto>>
{
    public async Task<List<MotherboardM2Dto>> Handle(
        GetMotherboardM2SlotsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListM2SlotsAsync(request.MotherboardId, cancellationToken);
    }
}
