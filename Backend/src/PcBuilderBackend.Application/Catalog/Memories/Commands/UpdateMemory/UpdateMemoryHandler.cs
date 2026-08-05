using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;

public class UpdateMemoryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateMemoryCommand, RamDto?>
{
    public async Task<RamDto?> Handle(UpdateMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Rams.FirstOrDefaultAsync(r => r.Id == request.Id && r.IsActive, cancellationToken);
        if (entity == null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.Color,
            request.DdrGeneration,
            request.RamFormFactor,
            request.RamRank,
            request.MemorySizePerStickGb,
            request.TotalMemorySizeGb,
            request.ModulesCount,
            request.MaxMemorySpeedMts,
            request.HeightMm);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<RamDto>(entity);
    }
}
