using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.ImportGraphicsCards;

public class ImportGraphicsCardsHandler(
    IApplicationDbContext context,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportGraphicsCardsHandler> logger)
    : IRequestHandler<ImportGraphicsCardsCommand, List<GraphicsCardDto>>
{
    public async Task<List<GraphicsCardDto>> Handle(
        ImportGraphicsCardsCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseGraphicsCardImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);
        await ActiveEntityGuard.EnsureGpusExist(
            lookup, rows.Select(r => r.GpuId), cancellationToken);

        var result = new List<GraphicsCard>();

        foreach (var row in rows)
        {
            var entity = new GraphicsCard(
                row.Name,
                row.ManufacturerId,
                row.GpuId,
                row.VideoMemoryGb,
                row.PcieSlotsUsed,
                row.PcieGeneration,
                row.IsLowProfile,
                row.LengthMm,
                row.WidthMm,
                row.HeightMm,
                row.PowerConsumptionWatts,
                row.PowerConnectorType,
                row.PowerConnectorCount);

            context.GraphicsCards.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.GraphicsCard);

        return [.. result.Select(mapper.Map<GraphicsCardDto>)];
    }
}
