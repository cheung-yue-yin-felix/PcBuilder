using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public class MotherboardUsbImportRow
{
    public int ParentRowNumber { get; init; }
    public UsbVersion UsbVersion { get; init; }
    public UsbType UsbType { get; init; }
    public int PortCount { get; init; }
}
