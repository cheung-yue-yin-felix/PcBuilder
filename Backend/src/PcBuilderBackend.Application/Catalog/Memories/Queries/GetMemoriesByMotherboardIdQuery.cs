using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public record GetMemoriesByMotherboardIdQuery(Guid MotherboardId) : IRequest<List<RamDto>>;