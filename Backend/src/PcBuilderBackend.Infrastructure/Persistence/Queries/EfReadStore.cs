using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public sealed class EfReadStore<TEntity, TDto>(PcBuilderDbContext db, IMapper mapper)
    : IReadStore<TDto>
    where TEntity : BaseEntity
{
    public Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<TDto>> ListAsync(CancellationToken cancellationToken)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var keySelector = Expression.Lambda<Func<TEntity, string>>(
            Expression.Property(parameter, "Name"),
            parameter);

        return db.Set<TEntity>()
            .AsNoTracking()
            .OrderBy(keySelector)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}