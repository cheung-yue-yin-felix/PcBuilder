using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PcBuilderBackend.Application.Common.Mappings;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class AppFixture : IDisposable
{
    public AppFixture()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase($"app-{Guid.NewGuid()}")
            .Options;

        Context = new TestApplicationDbContext(options);
        Manufacturer = new Manufacturer("AMD");
        Context.Manufacturers.Add(Manufacturer);
        Context.SaveChanges();

        Socket = new Socket(Manufacturer.Id, "AM5");
        Context.Sockets.Add(Socket);
        Context.SaveChanges();

        Chipset = new Chipset("B650", Manufacturer.Id, Socket.Id);
        CpuSeries = new CpuSeries(Manufacturer.Id, Socket.Id, "Ryzen 7000");
        GpuSeries = new GpuSeries(Manufacturer.Id, "GeForce RTX 40");
        Context.Chipsets.Add(Chipset);
        Context.CpuSeries.Add(CpuSeries);
        Context.GpuSeries.Add(GpuSeries);
        Context.SaveChanges();

        Gpu = new Gpu("RTX 4070", Manufacturer.Id, GpuSeries.Id);
        Context.Gpus.Add(Gpu);
        Context.SaveChanges();

        Mapper = new MapperConfiguration(
            cfg => cfg.AddMaps(typeof(ManufacturerProfile).Assembly),
            NullLoggerFactory.Instance).CreateMapper();

        Cache = new FakeCacheService();
        Lookup = new TestActiveEntityLookup(Context);
        Cpus = new TestCpuRepository(Context);
        UnitOfWork = new TestUnitOfWork(Context);
    }

    public TestApplicationDbContext Context { get; }
    public IMapper Mapper { get; }
    public FakeCacheService Cache { get; }
    public TestActiveEntityLookup Lookup { get; }
    public TestCpuRepository Cpus { get; }
    public TestUnitOfWork UnitOfWork { get; }
    public Manufacturer Manufacturer { get; }
    public Socket Socket { get; }
    public Chipset Chipset { get; }
    public CpuSeries CpuSeries { get; }
    public GpuSeries GpuSeries { get; }
    public Gpu Gpu { get; }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
