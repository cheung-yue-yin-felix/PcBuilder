using MediatR;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.DeleteManufacturer;

public record DeleteManufacturerCommand(Guid Id): IRequest<bool>;