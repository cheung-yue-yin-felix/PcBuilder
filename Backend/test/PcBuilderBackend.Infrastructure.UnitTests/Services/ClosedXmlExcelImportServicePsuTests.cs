using ClosedXML.Excel;
using FluentAssertions;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Infrastructure.Services;

namespace PcBuilderBackend.Infrastructure.UnitTests.Services;

public class ClosedXmlExcelImportServicePsuTests
{
    private readonly ClosedXmlExcelImportService _sut = new();
    private readonly Guid _manufacturerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [Fact]
    public async Task Parse_joins_cables_to_parent_rows()
    {
        await using var stream = CreateWorkbook(
            psuRows:
            [
                ["RM850x", _manufacturerId.ToString(), 850, "FullModular", "Atx", 160, 150, 86]
            ],
            cableRows:
            [
                [2, "Motherboard24Pin", 1, 1],
                [2, "Sata", 4, 4]
            ]);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows.Should().ContainSingle();
        rows[0].Name.Should().Be("RM850x");
        rows[0].ManufacturerId.Should().Be(_manufacturerId);
        rows[0].Wattage.Should().Be(850);
        rows[0].Modularity.Should().Be(PsuModularity.FullModular);
        rows[0].FormFactor.Should().Be(PsuFormFactor.Atx);
        rows[0].Cables.Should().HaveCount(2);
        rows[0].Cables.Should().Contain(c => c.Type == PsuCableType.Motherboard24Pin && c.CablesCount == 1);
        rows[0].Cables.Should().Contain(c => c.Type == PsuCableType.Sata && c.ConnectorsCount == 4);
    }

    [Fact]
    public async Task Parse_succeeds_without_cables_sheet()
    {
        await using var stream = CreateWorkbook(
            psuRows: [["SF750", _manufacturerId.ToString(), 750, "FullModular", "Sfx", 100, 125, 63.5]],
            cableRows: null);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows.Should().ContainSingle();
        rows[0].Cables.Should().BeEmpty();
        rows[0].HeightMm.Should().Be(63.5m);
    }

    [Fact]
    public async Task Parse_ignores_orphan_cable_rows()
    {
        await using var stream = CreateWorkbook(
            psuRows: [["RM850x", _manufacturerId.ToString(), 850, "FullModular", "Atx", 160, 150, 86]],
            cableRows: [[99, "Sata", 2, 4]]);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows[0].Cables.Should().BeEmpty();
    }

    [Fact]
    public async Task Parse_maps_invalid_guid_to_empty()
    {
        await using var stream = CreateWorkbook(
            psuRows: [["RM850x", "not-a-guid", 850, "FullModular", "Atx", 160, 150, 86]],
            cableRows: null);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows[0].ManufacturerId.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task Parse_skips_empty_rows_in_parent_and_child_sheets()
    {
        await using var stream = CreateWorkbook(
            psuRows:
            [
                ["RM850x", _manufacturerId.ToString(), 850, "FullModular", "Atx", 160, 150, 86],
                ["", "", "", "", "", "", "", ""],
                ["SF750", _manufacturerId.ToString(), 750, "FullModular", "Sfx", 100, 125, 63.5]
            ],
            cableRows:
            [
                [2, "Motherboard24Pin", 1, 1],
                ["", "", "", ""],
                [2, "Sata", 4, 4],
                [4, "Cpu4Plus4Pin", 1, 1]
            ]);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows.Should().HaveCount(2);
        rows[0].Name.Should().Be("RM850x");
        rows[0].Cables.Should().HaveCount(2);
        rows[1].Name.Should().Be("SF750");
        rows[1].Cables.Should().ContainSingle(c => c.Type == PsuCableType.Cpu4Plus4Pin);
    }

    [Fact]
    public async Task Parse_returns_empty_list_for_header_only_sheet()
    {
        await using var stream = CreateWorkbook(psuRows: [], cableRows: null);

        var rows = await _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        rows.Should().BeEmpty();
    }

    [Fact]
    public async Task Parse_throws_for_invalid_enum()
    {
        var stream = CreateWorkbook(
            psuRows: [["RM850x", _manufacturerId.ToString(), 850, "NotAModularity", "Atx", 160, 150, 86]],
            cableRows: null);

        try
        {
            var act = () => _sut.ParsePsuImportAsync(stream, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }

    [Fact]
    public async Task Parse_throws_when_psus_sheet_is_missing()
    {
        using var workbook = new XLWorkbook();
        workbook.Worksheets.Add("Other");
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        var act = () => _sut.ParsePsuImportAsync(stream, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    private static MemoryStream CreateWorkbook(object[][] psuRows, object[][]? cableRows)
    {
        using var workbook = new XLWorkbook();
        var psus = workbook.Worksheets.Add("Psus");
        WriteHeader(psus, ["Name", "ManufacturerId", "Wattage", "Modularity", "FormFactor", "LengthMm", "WidthMm", "HeightMm"]);
        for (var i = 0; i < psuRows.Length; i++)
            WriteRow(psus, i + 2, psuRows[i]);

        if (cableRows is not null)
        {
            var cables = workbook.Worksheets.Add("PsuCables");
            WriteHeader(cables, ["ParentRowNumber", "Type", "CablesCount", "ConnectorsCount"]);
            for (var i = 0; i < cableRows.Length; i++)
                WriteRow(cables, i + 2, cableRows[i]);
        }

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    private static void WriteHeader(IXLWorksheet sheet, string[] headers)
    {
        for (var i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).Value = headers[i];
    }

    private static void WriteRow(IXLWorksheet sheet, int row, object[] values)
    {
        for (var i = 0; i < values.Length; i++)
        {
            sheet.Cell(row, i + 1).Value = values[i] switch
            {
                int n => n,
                double d => d,
                decimal m => m,
                string s => s,
                _ => values[i].ToString() ?? string.Empty
            };
        }
    }
}
