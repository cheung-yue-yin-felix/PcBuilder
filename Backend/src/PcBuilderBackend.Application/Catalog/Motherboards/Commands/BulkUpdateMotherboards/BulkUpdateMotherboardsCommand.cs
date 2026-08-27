using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.UpdateMotherboard;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboards;

public record BulkUpdateMotherboardsCommand(List<UpdateMotherboardCommand> Motherboards) : IRequest<List<MotherboardDto>?>;