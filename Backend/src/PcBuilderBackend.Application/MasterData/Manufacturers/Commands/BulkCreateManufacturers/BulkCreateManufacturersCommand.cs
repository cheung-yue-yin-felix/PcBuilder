using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkCreateManufacturers;

public record BulkCreateManufacturersCommand(List<string> Names): IRequest<List<ManufacturerDto>>;