using FluentAssertions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Domain.UnitTests.Entities;

public class BaseEntityTests
{
    [Fact]
    public void New_entity_is_active_with_generated_id()
    {
        var entity = new Manufacturer("Corsair");

        entity.Id.Should().NotBe(Guid.Empty);
        entity.IsActive.Should().BeTrue();
        entity.ConcurrencyToken.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Deactivate_marks_entity_inactive()
    {
        var entity = new Manufacturer("Corsair");

        entity.Deactivate();

        entity.IsActive.Should().BeFalse();
        entity.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void Activate_restores_inactive_entity()
    {
        var entity = new Manufacturer("Corsair");
        entity.Deactivate();

        entity.Activate();

        entity.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Entities_with_same_id_are_equal()
    {
        var left = new Manufacturer("Corsair");
        var right = new Manufacturer("Seasonic");
        typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(right, left.Id);

        left.Equals(right).Should().BeTrue();
        left.GetHashCode().Should().Be(right.GetHashCode());
    }
}
