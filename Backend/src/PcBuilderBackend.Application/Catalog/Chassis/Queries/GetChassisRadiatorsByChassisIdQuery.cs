using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisRadiatorsByChassisIdQuery(Guid ChassisId)
    : IRequest<List<ChassisRadiatorDto>>;
