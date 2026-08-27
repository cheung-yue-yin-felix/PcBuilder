using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;

public record BulkCreateGraphicsCardsCommand(List<CreateGraphicsCardItem> Cards)
    : IRequest<List<GraphicsCardDto>>;

public record CreateGraphicsCardItem
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid GpuId { get; init; }
    public int VideoMemoryGb { get; init; }
    public int PcieSlotsUsed { get; init; }
    public PcieGeneration PcieGeneration { get; init; }
    public bool IsLowProfile { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public int PowerConsumptionWatts { get; init; }
    public PsuCableType PowerConnectorType { get; init; }
    public int PowerConnectorCount { get; init; }
}
