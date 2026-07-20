using MediatR;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.UpdateManufacturer;

public record UpdateManufacturerCommand(Guid Id, string Name) : IRequest<ManufacturerDto?>;