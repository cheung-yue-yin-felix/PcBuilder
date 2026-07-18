namespace PcBuilderBackend.Domain.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; protected set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();
}

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}