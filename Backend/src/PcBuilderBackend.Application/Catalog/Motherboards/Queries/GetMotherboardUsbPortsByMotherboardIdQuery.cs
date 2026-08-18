using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record GetMotherboardUsbPortsByMotherboardIdQuery(Guid MotherboardId)
    : IRequest<List<MotherboardUsbDto>>;
