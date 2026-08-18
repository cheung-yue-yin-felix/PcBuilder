namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public record MotherboardDto: MotherboardListItemDto
{
    public List<MotherboardPcieDto> PcieSlots { get; init; } = [];
    public List<MotherboardM2Dto> M2Slots { get; init; } = [];
    public List<MotherboardUsbDto> UsbPorts { get; init; } = [];
}
