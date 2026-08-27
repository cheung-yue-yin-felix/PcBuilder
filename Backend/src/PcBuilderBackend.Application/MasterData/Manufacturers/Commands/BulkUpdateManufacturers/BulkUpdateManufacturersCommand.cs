using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkUpdateManufacturers;

public record BulkUpdateManufacturersCommand(List<ManufacturerDto> Manufacturers): IRequest<List<ManufacturerDto>?>;