using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.ImportMotherboards;

public record ImportMotherboardsCommand(Stream Stream) : IRequest<List<MotherboardDto>>;
