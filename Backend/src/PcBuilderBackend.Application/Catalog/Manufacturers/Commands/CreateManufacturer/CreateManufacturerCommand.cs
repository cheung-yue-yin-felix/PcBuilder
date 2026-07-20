using MediatR;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.CreateManufacturer;

public record CreateManufacturerCommand(string Name) : IRequest<ManufacturerDto>;
