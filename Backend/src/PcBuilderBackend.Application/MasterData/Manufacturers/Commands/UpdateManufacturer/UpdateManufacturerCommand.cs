using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.UpdateManufacturer;

public record UpdateManufacturerCommand(Guid Id, string Name) : IRequest<ManufacturerDto?>;
