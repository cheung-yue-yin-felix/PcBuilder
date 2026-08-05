using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Extensions;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMemoriesQuery, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(GetMemoriesQuery request, CancellationToken cancellationToken)
    {
        return await context.Rams
            .AsNoTracking()
            .Where(x => x.IsActive)
            .WhereIf(!string.IsNullOrEmpty(request.Name), x => x.Name.Contains(request.Name!))
            .WhereIf(request.ManufacturerId.HasValue, x => x.ManufacturerId == request.ManufacturerId)
            .WhereIf(!string.IsNullOrEmpty(request.Color), x => x.Color == request.Color)
            .WhereIf(request.DdrGeneration.HasValue, x => x.DdrGeneration == request.DdrGeneration)
            .WhereIf(request.RamFormFactor.HasValue, x => x.RamFormFactor == request.RamFormFactor)
            .WhereIf(request.RamRank.HasValue, x => x.RamRank == request.RamRank)
            .WhereIf(request.MemorySizePerStickGb.HasValue, x => x.MemorySizePerStickGb == request.MemorySizePerStickGb)
            .WhereIf(request.TotalMemorySizeGb.HasValue, x => x.TotalMemorySizeGb == request.TotalMemorySizeGb)
            .WhereIf(request.ModulesCount.HasValue, x => x.ModulesCount == request.ModulesCount)
            .WhereIf(request.MaxMemorySpeedMts.HasValue, x => x.MaxMemorySpeedMts == request.MaxMemorySpeedMts)
            .WhereIf(request.MinHeightMm.HasValue, x => x.HeightMm >= request.MinHeightMm)
            .WhereIf(request.MaxHeightMm.HasValue, x => x.HeightMm <= request.MaxHeightMm)
            .ProjectTo<RamDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
