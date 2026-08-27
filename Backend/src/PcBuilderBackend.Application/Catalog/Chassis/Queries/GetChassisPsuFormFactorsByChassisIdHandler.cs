using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisPsuFormFactorsByChassisIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisPsuFormFactorsByChassisIdQuery, List<PsuFormFactor>>
{
    public async Task<List<PsuFormFactor>> Handle(
        GetChassisPsuFormFactorsByChassisIdQuery query,
        CancellationToken cancellationToken)
    {
        return await store.ListPsuFormFactorsAsync(query.ChassisId, cancellationToken);
    }
}
