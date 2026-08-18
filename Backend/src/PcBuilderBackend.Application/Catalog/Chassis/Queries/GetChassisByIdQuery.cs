using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisByIdQuery(Guid Id) : IRequest<ChassisDto?>;