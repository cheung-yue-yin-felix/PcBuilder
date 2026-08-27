using MediatR;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

public record BulkDeleteManufacturersCommand(List<Guid> ManufacturerIds): IRequest<bool>;