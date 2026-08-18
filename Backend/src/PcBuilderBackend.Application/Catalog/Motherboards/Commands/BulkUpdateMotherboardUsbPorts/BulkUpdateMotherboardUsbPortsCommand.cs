using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardUsbPorts;

public record BulkUpdateMotherboardUsbPortsCommand(
    Guid MotherboardId,
    List<MotherboardUsbDto> UsbPorts) : IRequest<List<MotherboardUsbDto>?>;
