using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisPcieSlotsByChassisIdQuery(Guid ChassisId)
    : IRequest<List<ChassisPcieSlotDto>>;
