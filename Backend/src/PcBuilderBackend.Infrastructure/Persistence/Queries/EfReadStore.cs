using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public sealed class EfReadStore<TEntity, TDto>(PcBuilderDbContext db, IMapper mapper)
    : IReadStore<TEntity, TDto>
    where TEntity : BaseEntity
{
    public Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<TDto>> ListAsync(CancellationToken cancellationToken) =>
        db.Set<TEntity>()
            .AsNoTracking()
            .OrderBy(x => EF.Property<string>(x, "Name"))
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
