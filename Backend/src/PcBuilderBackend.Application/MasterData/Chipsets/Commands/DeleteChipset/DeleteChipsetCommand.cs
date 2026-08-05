using MediatR;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.DeleteChipset;

public record DeleteChipsetCommand(Guid Id): IRequest<bool>;