using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class PcBuildPart : BaseEntity
{
    public Guid PcBuildId { get; private set; }
    public PcBuild? PcBuild { get; private set; }
    public PcBuildPartType Type { get; private set; }
    public Guid PartId { get; private set; }
    public int Quantity { get; private set; }

    protected PcBuildPart()
    {
    }

    public PcBuildPart(Guid pcBuildId, PcBuildPartType type, Guid partId, int quantity)
    {
        SetSpecs(pcBuildId, type, partId, quantity);
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

    private void SetSpecs(Guid pcBuildId, PcBuildPartType type, Guid partId, int quantity)
    {
        if (pcBuildId == Guid.Empty)
            throw new ArgumentException("PC Build ID is required.", nameof(pcBuildId));

        if (!Enum.IsDefined(type))
            throw new ArgumentException("Part type is invalid.", nameof(type));

        if (partId == Guid.Empty)
            throw new ArgumentException("Part ID is required.", nameof(partId));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        PcBuildId = pcBuildId;
        Type = type;
        PartId = partId;
        Quantity = quantity;
    }
}
