using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkUpdateMemories;

public record BulkUpdateMemoriesCommand(List<UpdateMemoryCommand> Memories) : IRequest<List<RamDto>>;