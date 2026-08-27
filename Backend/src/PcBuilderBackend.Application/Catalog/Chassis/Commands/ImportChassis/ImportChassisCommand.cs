using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.ImportChassis;

public record ImportChassisCommand(Stream Stream) : IRequest<List<ChassisDto>>;
