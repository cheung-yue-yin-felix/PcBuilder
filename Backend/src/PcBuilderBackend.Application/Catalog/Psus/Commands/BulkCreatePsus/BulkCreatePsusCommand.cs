using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.Psus;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkCreatePsus;

public record BulkCreatePsusCommand(List<CreatePsuItem> Psus) : IRequest<List<PsuDto>>;

public record CreatePsuItem : IPsuFields, IPsuCables
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public int Wattage { get; init; }
    public PsuModularity Modularity { get; init; }
    public PsuFormFactor FormFactor { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public List<PsuCableDto> Cables { get; init; } = [];
}
