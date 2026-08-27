using FluentAssertions;
using PcBuilderBackend.Application.Common.Extensions;

namespace PcBuilderBackend.Application.UnitTests.Common;

public class QueryableExtensionsTests
{
    private sealed record Item(string Name, int Wattage);

    [Fact]
    public void In_memory_sort_is_case_insensitive()
    {
        var items = new[] { new Item("SF750", 750), new Item("RM850x", 850) };

        var sorted = items.ApplySorting(["name"]).Cast<Item>().ToList();

        sorted.Select(x => x.Name).Should().Equal("RM850x", "SF750");
    }

    [Fact]
    public void In_memory_sort_descending_by_numeric_property()
    {
        var items = new[] { new Item("A", 100), new Item("B", 300), new Item("C", 200) };

        var sorted = items.ApplySorting(["wattage"], "desc").Cast<Item>().ToList();

        sorted.Select(x => x.Wattage).Should().Equal(300, 200, 100);
    }
}
