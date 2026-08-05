using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardPcieSlots;

public record BulkUpdateMotherboardPcieSlotsCommand(
    Guid MotherboardId,
    List<MotherboardPcieDto> PcieSlots) : IRequest<List<MotherboardPcieDto>?>;
