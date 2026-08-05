using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public record GetGraphicsCardsQuery(
    int PageIndex = 0,
    int PageSize = 10,
    string? Name = null,
    Guid? ManufacturerId = null,
    Guid? GpuId = null,
    PcieGeneration? PcieGeneration = null,
    int? MinVideoMemoryGb = null,
    int? MaxVideoMemoryGb = null,
    decimal? MaxLengthMm = null,
    decimal? MaxPowerConsumptionWatts = null) : IRequest<PagedResult<GraphicsCardListItemDto>>;
