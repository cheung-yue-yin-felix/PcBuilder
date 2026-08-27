
using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardByIdHandler(IMotherboardReadStore store)
    : IRequestHandler<GetMotherboardByIdQuery, MotherboardDto?>
{
    public async Task<MotherboardDto?> Handle(GetMotherboardByIdQuery request, CancellationToken cancellationToken)
    {
        return await store.GetByIdAsync(request.Id, cancellationToken);
    }
}
