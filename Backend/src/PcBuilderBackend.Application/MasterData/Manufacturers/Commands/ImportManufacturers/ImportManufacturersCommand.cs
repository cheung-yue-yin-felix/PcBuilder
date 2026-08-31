using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.ImportManufacturers;

public record ImportManufacturersCommand(Stream Stream) : IRequest<List<ManufacturerDto>>;
