using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisPsuFormFactorsByChassisIdQuery(Guid ChassisId)
    : IRequest<List<PsuFormFactor>>;
