using MediatR;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.DeleteMemory;

public record DeleteMemoryCommand(Guid Id) : IRequest<bool>;
