using MediatR;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

public record BulkDeleteManufacturersCommand(List<Guid> Ids): IRequest<bool>;