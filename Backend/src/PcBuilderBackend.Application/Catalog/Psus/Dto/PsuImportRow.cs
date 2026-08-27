using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Dto;

public class PsuImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public int Wattage { get; init; }
    public PsuModularity Modularity { get; init; }
    public PsuFormFactor FormFactor { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public List<PsuCableImportRow> Cables { get; init; } = [];
}
