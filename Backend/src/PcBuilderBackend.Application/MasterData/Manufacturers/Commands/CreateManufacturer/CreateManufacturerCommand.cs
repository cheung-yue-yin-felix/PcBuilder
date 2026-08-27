using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.CreateManufacturer;

public record CreateManufacturerCommand(string Name) : IRequest<ManufacturerDto>;
