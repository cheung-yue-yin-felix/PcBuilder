using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Build;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public class PcBuildReadStore(PcBuilderDbContext context, IMapper mapper) : IPcBuildReadStore
{
    public Task<PcBuildDto?> GetByIdAsync(Guid id) =>
        context.PcBuilds
            .Where(p => p.Id == id)
            .ProjectTo<PcBuildDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    
    public Task<PagedResult<PcBuildListItemDto>> ListAsync(
        PagedRequest request,
        CancellationToken cancellationToken) =>
        context.PcBuilds
            .AsNoTracking()
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<PcBuild, PcBuildListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
}