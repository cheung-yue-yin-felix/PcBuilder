using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class TestReadStore<TEntity, TDto>(TestApplicationDbContext db, IMapper mapper)
    : IReadStore<TDto>
    where TEntity : BaseEntity
{
    public async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? default : mapper.Map<TDto>(entity);
    }

    public async Task<List<TDto>> ListAsync(CancellationToken cancellationToken)
    {
        var entities = await db.Set<TEntity>().ToListAsync(cancellationToken);
        return entities.Select(mapper.Map<TDto>).ToList();
    }
}
