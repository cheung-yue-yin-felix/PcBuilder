using FluentAssertions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Domain.UnitTests.MasterData;

public class MasterDataEntityTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Socket_requires_manufacturer()
    {
        var socket = new Socket(ManufacturerId, "AM5");

        socket.Name.Should().Be("AM5");
        socket.ManufacturerId.Should().Be(ManufacturerId);

        var act = () => new Socket(Guid.Empty, "AM5");
        act.Should().Throw<ArgumentException>().WithParameterName("manufacturerId");
    }

    [Fact]
    public void Chipset_requires_socket()
    {
        var chipset = new Chipset("B650", ManufacturerId, Guid.NewGuid());
        chipset.Name.Should().Be("B650");

        var act = () => new Chipset("B650", ManufacturerId, Guid.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CpuSeries_requires_socket()
    {
        var series = new CpuSeries(ManufacturerId, Guid.NewGuid(), "Ryzen 7000");
        series.Name.Should().Be("Ryzen 7000");

        var act = () => new CpuSeries(ManufacturerId, Guid.Empty, "Ryzen 7000");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Gpu_and_gpu_series_require_ids()
    {
        var series = new GpuSeries(ManufacturerId, "GeForce RTX 40");
        series.Name.Should().Be("GeForce RTX 40");

        var gpu = new Gpu("RTX 4070", ManufacturerId, series.Id);
        gpu.SeriesId.Should().Be(series.Id);

        var act = () => new Gpu("RTX 4070", ManufacturerId, Guid.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Chipset_and_gpu_update_related_ids()
    {
        var chipset = new Chipset("B650", ManufacturerId, Guid.NewGuid());
        var nextSocket = Guid.NewGuid();
        chipset.UpdateSpecs(nextSocket);
        chipset.SocketId.Should().Be(nextSocket);

        var gpu = new Gpu("RTX 4070", ManufacturerId, Guid.NewGuid());
        var nextSeries = Guid.NewGuid();
        gpu.UpdateSeries(nextSeries);
        gpu.SeriesId.Should().Be(nextSeries);
    }
}
