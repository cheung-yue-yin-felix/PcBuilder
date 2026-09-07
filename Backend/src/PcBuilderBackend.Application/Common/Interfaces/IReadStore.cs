namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IReadStore<TDto>
{
    Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TDto>> ListAsync(CancellationToken cancellationToken);
}