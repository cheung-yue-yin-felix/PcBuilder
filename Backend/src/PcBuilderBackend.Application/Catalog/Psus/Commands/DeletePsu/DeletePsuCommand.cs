using MediatR;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.DeletePsu;

public record DeletePsuCommand(Guid Id) : IRequest<bool>;
