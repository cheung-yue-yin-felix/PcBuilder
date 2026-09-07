using FluentAssertions;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.UnitTests.Support;

namespace PcBuilderBackend.Application.UnitTests.Common;

public class ActiveEntityGuardTests : IDisposable
{
    private readonly AppFixture _fx = new();

    public void Dispose() => _fx.Dispose();

    [Fact]
    public async Task Ensure_manufacturers_accepts_known_ids_and_empty_sets()
    {
        var empty = () => ActiveEntityGuard.EnsureManufacturersExist(_fx.Lookup, [], CancellationToken.None);
        var known = () => ActiveEntityGuard.EnsureManufacturersExist(
            _fx.Lookup, [_fx.Manufacturer.Id], CancellationToken.None);

        await empty.Should().NotThrowAsync();
        await known.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Ensure_manufacturers_rejects_unknown_ids()
    {
        var act = () => ActiveEntityGuard.EnsureManufacturersExist(
            _fx.Lookup, [Guid.NewGuid()], CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Manufacturer*");
    }

    [Fact]
    public async Task Ensure_sockets_and_chipsets()
    {
        var sockets = () => ActiveEntityGuard.EnsureSocketsExist(
            _fx.Lookup, [_fx.Socket.Id], CancellationToken.None);
        var chipsets = () => ActiveEntityGuard.EnsureChipsetsExist(
            _fx.Lookup, [_fx.Chipset.Id], CancellationToken.None);
        var gpus = () => ActiveEntityGuard.EnsureGpusExist(
            _fx.Lookup, [_fx.Gpu.Id], CancellationToken.None);

        await sockets.Should().NotThrowAsync();
        await chipsets.Should().NotThrowAsync();
        await gpus.Should().NotThrowAsync();
    }
}
