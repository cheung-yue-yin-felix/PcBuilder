using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record GetMotherboardsQuery : IRequest<List<MotherboardDto>>
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public Guid? SocketId { get; init; }
    public Guid? ChipsetId { get; init; }
    public DdrGeneration? DdrGeneration { get; init; }
    public RamFormFactor? RamFormFactor { get; init; }
    public MbFormFactor? FormFactor { get; init; }
    public bool? WifiEnabled { get; init; }
    public bool? BluetoothEnabled { get; init; }
}
