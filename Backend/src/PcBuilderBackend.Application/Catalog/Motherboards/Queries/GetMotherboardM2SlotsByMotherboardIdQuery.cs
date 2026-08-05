using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record GetMotherboardM2SlotsByMotherboardIdQuery(Guid MotherboardId)
    : IRequest<List<MotherboardM2Dto>>;
