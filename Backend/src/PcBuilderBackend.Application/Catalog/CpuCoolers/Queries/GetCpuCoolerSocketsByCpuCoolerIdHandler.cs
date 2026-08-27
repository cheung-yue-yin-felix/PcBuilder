using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public class GetCpuCoolerSocketsByCpuCoolerIdHandler(ICpuCoolerReadStore store)
    : IRequestHandler<GetCpuCoolerSocketsByCpuCoolerIdQuery, List<CpuCoolerSocketDto>>
{
    public async Task<List<CpuCoolerSocketDto>> Handle(
        GetCpuCoolerSocketsByCpuCoolerIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListCpuCoolerSockets(request.CpuCoolerId, cancellationToken);
    }
}
