using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkCreateMotherboards;

public record BulkCreateMotherboardCommand(List<MotherboardDto> Motherboards) : IRequest<List<MotherboardDto>>;