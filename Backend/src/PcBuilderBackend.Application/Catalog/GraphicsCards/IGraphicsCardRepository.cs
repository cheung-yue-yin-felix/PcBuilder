using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards;

public interface IGraphicsCardRepository
{
    Task<GraphicsCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<GraphicsCard?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<GraphicsCard>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(GraphicsCard graphicsCard);
}