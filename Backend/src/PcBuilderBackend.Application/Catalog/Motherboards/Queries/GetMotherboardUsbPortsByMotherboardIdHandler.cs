using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardUsbPortsByMotherboardIdHandler(IMotherboardReadStore store)
    : IRequestHandler<GetMotherboardUsbPortsByMotherboardIdQuery, List<MotherboardUsbDto>>
{
    public async Task<List<MotherboardUsbDto>> Handle(
        GetMotherboardUsbPortsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListUsbPortsAsync(request.MotherboardId, cancellationToken);
    }
}
