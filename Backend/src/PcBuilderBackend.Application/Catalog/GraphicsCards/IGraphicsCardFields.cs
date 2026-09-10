using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards;

public interface IGraphicsCardFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    Guid GpuId { get; }
    int VideoMemoryGb { get; }
    int PcieSlotsUsed { get; }
    PcieGeneration PcieGeneration { get; }
    decimal LengthMm { get; }
    decimal WidthMm { get; }
    decimal HeightMm { get; }
    int PowerConsumptionWatts { get; }
    PsuCableType PowerConnectorType { get; }
    int PowerConnectorCount { get; }
}
