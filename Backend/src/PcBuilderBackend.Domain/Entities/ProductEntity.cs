namespace PcBuilderBackend.Domain.Entities;

public abstract class ProductEntity: NamedEntity
{
    public Guid ManufacturerId { get; private set; }
    public virtual Manufacturer Manufacturer { get; protected set; } = null!;

    internal void UpdateManufacturer(Guid manufacturerId)
    {
        SetManufacturer(manufacturerId);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    internal void SetManufacturer(Guid manufacturerId)
    {
        if (manufacturerId == Guid.Empty)
            throw new ArgumentException("Manufacturer id cannot be empty.", nameof(manufacturerId));
        
        ManufacturerId = manufacturerId;
    }
}