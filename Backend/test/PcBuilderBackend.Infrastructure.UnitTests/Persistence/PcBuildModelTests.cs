using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Infrastructure.Persistence;

namespace PcBuilderBackend.Infrastructure.UnitTests.Persistence;

public class PcBuildModelTests
{
    [Fact]
    public void Parts_use_one_table_and_split_by_type()
    {
        using var db = CreateContext();

        var part = db.Model.FindEntityType(typeof(PcBuildPart))!;
        part.GetTableName().Should().Be("PcBuildParts");
        part.GetForeignKeys()
            .Where(fk => fk.PrincipalEntityType.ClrType == typeof(PcBuild))
            .SelectMany(fk => fk.Properties.Select(p => p.Name))
            .Should()
            .Equal("PcBuildId");

        var build = db.Model.FindEntityType(typeof(PcBuild))!;
        build.FindNavigation("Parts").Should().NotBeNull();
        build.FindNavigation(nameof(PcBuild.StorageDevices)).Should().BeNull();

        part.GetIndexes()
            .Should()
            .Contain(index => index.IsUnique
                && index.Properties.Select(p => p.Name)
                    .SequenceEqual(new[] { "PcBuildId", "Type", "PartId" }));
    }

    [Fact]
    public void Motherboard_m2_slots_are_not_unique_on_key_and_generation()
    {
        using var db = CreateContext();

        var m2 = db.Model.FindEntityType(typeof(MotherboardM2))!;
        m2.GetIndexes()
            .Should()
            .NotContain(index => index.IsUnique);
        m2.GetIndexes()
            .Should()
            .Contain(index => index.Properties.Select(p => p.Name).SequenceEqual(new[] { "MotherboardId" }));
    }

    private static PcBuilderDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PcBuilderDbContext>()
            .UseNpgsql("Host=localhost;Database=pcbuilder_model_check;Username=x;Password=x")
            .Options;
        return new PcBuilderDbContext(options);
    }
}
