using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisDriveBaysByChassisIdQuery(Guid ChassisId)
    : IRequest<List<ChassisDriveBayDto>>;
