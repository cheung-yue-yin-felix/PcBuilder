using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkUpdateMemories;

public record BulkUpdateMemoriesCommand(List<RamDto> Memories) : IRequest<List<RamDto>>;