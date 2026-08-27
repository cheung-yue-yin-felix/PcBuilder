using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisDriveBaysByChassisIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisDriveBaysByChassisIdQuery, List<ChassisDriveBayDto>>
{
    public async Task<List<ChassisDriveBayDto>> Handle(
        GetChassisDriveBaysByChassisIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListDriveBaysAsync(request.ChassisId, cancellationToken);
    }
}
