using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkCreateMemories;

public record BulkCreateMemoriesCommand(List<RamDto> Memories) : IRequest<List<RamDto>>;