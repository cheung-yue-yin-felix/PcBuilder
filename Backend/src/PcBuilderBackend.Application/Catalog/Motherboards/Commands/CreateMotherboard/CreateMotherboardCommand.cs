using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.CreateMotherboard;

public record CreateMotherboardCommand : IRequest<MotherboardDto>
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
    public Guid ChipsetId { get; init; }
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
    public List<MotherboardPcieDto> PcieSlots { get; init; } = [];
    public List<MotherboardM2Dto> M2Slots { get; init; } = [];
    public List<MotherboardUsbDto> UsbPorts { get; init; } = [];
}
