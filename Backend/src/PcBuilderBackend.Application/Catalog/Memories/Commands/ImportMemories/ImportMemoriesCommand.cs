using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.ImportMemories;

public record ImportMemoriesCommand(Stream Stream) : IRequest<List<RamDto>>;
