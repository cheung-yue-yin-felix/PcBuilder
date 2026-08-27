using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisRadiatorsByChassisIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisRadiatorsByChassisIdQuery, List<ChassisRadiatorDto>>
{
    public async Task<List<ChassisRadiatorDto>> Handle(
        GetChassisRadiatorsByChassisIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListRadiatorsAsync(request.ChassisId, cancellationToken);
    }
}
