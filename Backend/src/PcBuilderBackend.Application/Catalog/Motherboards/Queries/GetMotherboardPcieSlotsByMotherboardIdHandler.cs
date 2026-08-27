using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardPcieSlotsByMotherboardIdHandler(IMotherboardReadStore store)
    : IRequestHandler<GetMotherboardPcieSlotsByMotherboardIdQuery, List<MotherboardPcieDto>>
{
    public async Task<List<MotherboardPcieDto>> Handle(
        GetMotherboardPcieSlotsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListPcieSlotsAsync(request.MotherboardId, cancellationToken);
    }
}
