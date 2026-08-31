using FluentAssertions;
using NSubstitute;
using PcBuilderBackend.Application.Catalog.Psus;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Catalog.Psus.Queries;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.UnitTests.Catalog.Psus;

public class PsuQueryHandlerTests
{
    [Fact]
    public async Task Get_by_id_returns_store_result()
    {
        var id = Guid.NewGuid();
        var dto = new PsuDto
        {
            Id = id,
            Name = "RM850x",
            ManufacturerName = "Corsair",
            Cables = [new PsuCableDto { Type = PsuCableType.Motherboard24Pin, CablesCount = 1, ConnectorsCount = 1 }]
        };
        var store = Substitute.For<IPsuReadStore>();
        store.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(dto);
        store.GetByIdAsync(Arg.Is<Guid>(x => x != id), Arg.Any<CancellationToken>()).Returns((PsuDto?)null);

        var handler = new GetPsuByIdHandler(store);
        (await handler.Handle(new GetPsuByIdQuery(id), CancellationToken.None))!.Name.Should().Be("RM850x");
        (await handler.Handle(new GetPsuByIdQuery(Guid.NewGuid()), CancellationToken.None)).Should().BeNull();
    }

    [Fact]
    public async Task List_filter_and_cables_delegate_to_store()
    {
        var store = Substitute.For<IPsuReadStore>();
        var page = new PagedResult<PsuListItemDto>
        {
            PageIndex = 0,
            PageSize = 1,
            TotalCount = 2,
            Items = [new PsuListItemDto { Name = "RM850x" }]
        };
        store.ListAsync(Arg.Any<PagedRequest>(), Arg.Any<CancellationToken>()).Returns(page);
        store.FilterAsync(Arg.Any<PagedRequest<PsuFilter>>(), Arg.Any<CancellationToken>()).Returns(page);
        store.ListCablesAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns([new PsuCableDto { Type = PsuCableType.Sata, CablesCount = 2, ConnectorsCount = 4 }]);

        (await new GetPsusHandler(store).Handle(
                new GetPsusQuery(new PagedRequest(PageIndex: 0, PageSize: 1, SortBy: "name")),
                CancellationToken.None))
            .Items.Should().ContainSingle(x => x.Name == "RM850x");

        (await new FilterPsusHandler(store).Handle(
                new FilterPsusQuery(new PagedRequest<PsuFilter>(new PsuFilter { Name = "RM" })),
                CancellationToken.None))
            .Items.Should().ContainSingle();

        (await new GetPsuCablesByPsuIdHandler(store).Handle(
                new GetPsuCablesByPsuIdQuery(Guid.NewGuid()),
                CancellationToken.None))
            .Should().ContainSingle(c => c.Type == PsuCableType.Sata);
    }
}
