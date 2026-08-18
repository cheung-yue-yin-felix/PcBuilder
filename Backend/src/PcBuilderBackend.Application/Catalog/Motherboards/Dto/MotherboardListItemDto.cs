using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public record MotherboardListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string ManufacturerName { get; init; } = string.Empty;
    public Guid SocketId { get; init; }
    public string SocketName { get; init; } = string.Empty;
    public Guid ChipsetId { get; init; }
    public string ChipsetName { get; init; } = string.Empty;
    public int RamSlots { get; init; }
    public int MaxMemoryGb { get; init; }
    public int MaxDimmSizeGb { get; init; }
    public int SataPorts { get; init; }
    public int FanConnectors { get; init; }
    public int EpsConnectors { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public DdrGeneration DdrGeneration { get; init; }
    public RamFormFactor RamFormFactor { get; init; }
    public MbFormFactor FormFactor { get; init; }
    public bool WifiEnabled { get; init; }
    public bool BluetoothEnabled { get; init; }
}