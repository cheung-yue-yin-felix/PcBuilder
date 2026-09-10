using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PcBuilderBackend.Application.Catalog.Psus.Commands.CreatePsu;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Mappings;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class PsuCatalogFixture : IDisposable
{
    public PsuCatalogFixture()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase($"psu-catalog-{Guid.NewGuid()}")
            .Options;

        Context = new TestApplicationDbContext(options);
        Manufacturer = new Manufacturer("Corsair");
        Context.Manufacturers.Add(Manufacturer);
        Context.SaveChanges();

        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PsuProfile>(), NullLoggerFactory.Instance)
            .CreateMapper();
        Lookup = new TestActiveEntityLookup(Context);
        Psus = new TestPsuRepository(Context);
        UnitOfWork = new TestUnitOfWork(Context);
    }

    public TestApplicationDbContext Context { get; }
    public IMapper Mapper { get; }
    public TestActiveEntityLookup Lookup { get; }
    public TestPsuRepository Psus { get; }
    public TestUnitOfWork UnitOfWork { get; }
    public Manufacturer Manufacturer { get; }

    public CreatePsuCommand ValidCreate(string name = "RM850x", int wattage = 850) => new()
    {
        Name = name,
        ManufacturerId = Manufacturer.Id,
        Wattage = wattage,
        Modularity = PsuModularity.FullModular,
        FormFactor = PsuFormFactor.Atx,
        LengthMm = 160,
        WidthMm = 150,
        HeightMm = 86,
        Cables =
        [
            new PsuCableDto
            {
                Type = PsuCableType.Motherboard24Pin,
                CablesCount = 1,
                ConnectorsCount = 1
            }
        ]
    };

    public Psu SeedPsu(string name = "RM850x", int wattage = 850, PsuFormFactor formFactor = PsuFormFactor.Atx,
        decimal lengthMm = 160)
    {
        var psu = new Psu(
            name,
            Manufacturer.Id,
            new PsuSpecs
            {
                Wattage = wattage,
                Modularity = PsuModularity.FullModular,
                FormFactor = formFactor,
                LengthMm = lengthMm,
                WidthMm = 150,
                HeightMm = 86
            });
        Context.Psus.Add(psu);
        Context.SaveChanges();
        Context.Entry(psu).Reference(x => x.Manufacturer).Load();
        return psu;
    }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
