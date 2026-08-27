using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisPcieSlotsByChassisIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisPcieSlotsByChassisIdQuery, List<ChassisPcieSlotDto>>
{
    public async Task<List<ChassisPcieSlotDto>> Handle(
        GetChassisPcieSlotsByChassisIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListPcieSlotsAsync(request.ChassisId, cancellationToken);
    }
}
