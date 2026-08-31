using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IReadStore<TEntity, TDto> where TEntity : BaseEntity
{
    Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TDto>> ListAsync(CancellationToken cancellationToken);
}