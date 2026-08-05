using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkUpdateMemories;

public class BulkUpdateMemoriesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<BulkUpdateMemoriesCommand, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(BulkUpdateMemoriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<RamDto>();
        foreach (var memoryDto in request.Memories)
        {
            var entity = await context.Rams.FirstOrDefaultAsync(r => r.Id == memoryDto.Id && r.IsActive, cancellationToken);
            if (entity is null) return result;

            entity.Rename(memoryDto.Name);
            entity.UpdateManufacturer(memoryDto.ManufacturerId);
            entity.UpdateSpecs(
                memoryDto.Color,
                memoryDto.DdrGeneration,
                memoryDto.RamFormFactor,
                memoryDto.RamRank,
                memoryDto.MemorySizePerStickGb,
                memoryDto.TotalMemorySizeGb,
                memoryDto.ModulesCount,
                memoryDto.MaxMemorySpeedMts,
                memoryDto.HeightMm
            );
            result.Add(mapper.Map<RamDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}