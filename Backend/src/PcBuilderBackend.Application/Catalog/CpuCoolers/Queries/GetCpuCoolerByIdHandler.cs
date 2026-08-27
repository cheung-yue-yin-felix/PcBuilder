using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public class GetCpuCoolerByIdHandler(ICpuCoolerReadStore store)
    : IRequestHandler<GetCpuCoolerByIdQuery, CpuCoolerDto?>
{
    public async Task<CpuCoolerDto?> Handle(GetCpuCoolerByIdQuery request, CancellationToken cancellationToken)
    {
        return await store.GetByIdAsync(request.Id, cancellationToken);
    }
}
