namespace PcBuilderBackend.Domain.Entities;

public class UserBuildPart
{
    public Guid PartId { get; private set; }
    public int Quantity { get; private set; }

    protected UserBuildPart()
    {
    }

    public UserBuildPart(Guid partId, int quantity)
    {
        SetSpecs(partId, quantity);
    }

    public void Increase(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        Quantity += quantity;
    }

    public void Decrease(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        if (quantity >= Quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be less than the current quantity.");

        Quantity -= quantity;
    }

    private void SetSpecs(Guid partId, int quantity)
    {
        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", nameof(partId));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        PartId = partId;
        Quantity = quantity;
    }
}
