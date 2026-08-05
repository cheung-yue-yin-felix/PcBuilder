using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboards;

public record BulkUpdateMotherboardsCommand(List<MotherboardDto> Motherboards) : IRequest<List<MotherboardDto>>;