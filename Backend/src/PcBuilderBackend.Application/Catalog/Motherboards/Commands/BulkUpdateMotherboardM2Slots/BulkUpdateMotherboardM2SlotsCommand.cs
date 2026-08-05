using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardM2Slots;

public record BulkUpdateMotherboardM2SlotsCommand(
    Guid MotherboardId,
    List<MotherboardM2Dto> M2Slots) : IRequest<List<MotherboardM2Dto>?>;
