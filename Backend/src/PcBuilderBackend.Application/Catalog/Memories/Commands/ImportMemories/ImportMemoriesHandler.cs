using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.ImportMemories;

public class ImportMemoriesHandler(IRamRepository memories, IUnitOfWork unitOfWork, IActiveEntityLookup lookup, IExcelImportService excel, IMapper mapper)
    : IRequestHandler<ImportMemoriesCommand, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(ImportMemoriesCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParseRamImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<Ram>();

        foreach (var row in rows)
        {
            var entity = new Ram(
                row.Name,
                row.ManufacturerId,
                row.Color,
                row.DdrGeneration,
                row.RamFormFactor,
                row.RamRank,
                row.MemorySizePerStickGb,
                row.TotalMemorySizeGb,
                row.ModulesCount,
                row.MaxMemorySpeedMts,
                row.HeightMm);

            memories.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return [.. result.Select(mapper.Map<RamDto>)];
    }
}
