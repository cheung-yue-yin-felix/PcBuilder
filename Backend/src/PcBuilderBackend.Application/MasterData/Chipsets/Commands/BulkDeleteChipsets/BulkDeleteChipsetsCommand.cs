using MediatR;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;

public record BulkDeleteChipsetsCommand(List<Guid> Ids) : IRequest<bool>;
