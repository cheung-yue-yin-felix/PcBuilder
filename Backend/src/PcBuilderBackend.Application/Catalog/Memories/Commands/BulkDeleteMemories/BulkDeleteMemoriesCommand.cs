using MediatR;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkDeleteMemories;

public record BulkDeleteMemoriesCommand(List<Guid> MemoryIds) : IRequest<bool>;