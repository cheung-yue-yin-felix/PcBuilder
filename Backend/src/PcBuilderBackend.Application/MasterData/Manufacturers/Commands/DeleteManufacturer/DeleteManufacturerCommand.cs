using MediatR;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.DeleteManufacturer;

public record DeleteManufacturerCommand(Guid Id): IRequest<bool>;
