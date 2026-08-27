using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisFanMountsByChassisIdQuery(Guid ChassisId)
    : IRequest<List<ChassisFanMountDto>>;
