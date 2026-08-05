using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record GetMotherboardPcieSlotsByMotherboardIdQuery(Guid MotherboardId)
    : IRequest<List<MotherboardPcieDto>>;
