using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisFanMountsByChassisIdHandler(IChassisReadStore chassisReadStore)
    : IRequestHandler<GetChassisFanMountsByChassisIdQuery, List<ChassisFanMountDto>>
{
    public async Task<List<ChassisFanMountDto>> Handle(
        GetChassisFanMountsByChassisIdQuery request,
        CancellationToken cancellationToken)
    {
        return await chassisReadStore.ListFanMountsAsync(request.ChassisId, cancellationToken);
    }
}
