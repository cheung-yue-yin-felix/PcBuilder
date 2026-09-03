using ClosedXML.Excel;
using FluentAssertions;
using PcBuilderBackend.Infrastructure.Services;

namespace PcBuilderBackend.Infrastructure.UnitTests.Services;

public class ClosedXmlExcelImportServiceMasterDataTests
{
    private readonly ClosedXmlExcelImportService _sut = new();
    private readonly Guid _id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [Fact]
    public async Task Parse_manufacturer_socket_chipset_and_series_sheets()
    {
        await using var manufacturers = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Manufacturers");
            Write(sheet, 1, ["Name"]);
            Write(sheet, 2, ["Intel"]);
        });
        (await _sut.ParseManufacturerImportAsync(manufacturers, CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "Intel" && x.RowNumber == 2);

        await using var sockets = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Sockets");
            Write(sheet, 1, ["Name", "ManufacturerId"]);
            Write(sheet, 2, ["AM5", _id.ToString()]);
        });
        (await _sut.ParseSocketImportAsync(sockets, CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "AM5" && x.ManufacturerId == _id);

        await using var chipsets = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Chipsets");
            Write(sheet, 1, ["Name", "ManufacturerId", "SocketId"]);
            Write(sheet, 2, ["B650", _id.ToString(), _id.ToString()]);
        });
        (await _sut.ParseChipsetImportAsync(chipsets, CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "B650" && x.SocketId == _id);

        await using var cpuSeries = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("CpuSeries");
            Write(sheet, 1, ["Name", "ManufacturerId", "SocketId"]);
            Write(sheet, 2, ["Ryzen 7000", _id.ToString(), _id.ToString()]);
        });
        (await _sut.ParseCpuSeriesImportAsync(cpuSeries, CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "Ryzen 7000");

        await using var gpuSeries = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("GpuSeries");
            Write(sheet, 1, ["Name", "ManufacturerId"]);
            Write(sheet, 2, ["RTX 40", _id.ToString()]);
        });
        (await _sut.ParseGpuSeriesImportAsync(gpuSeries, CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "RTX 40" && x.ManufacturerId == _id);
    }

    [Fact]
    public async Task Parse_skips_empty_and_whitespace_rows()
    {
        await using var manufacturers = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Manufacturers");
            Write(sheet, 1, ["Name"]);
            Write(sheet, 2, ["Intel"]);
            Write(sheet, 3, [""]);
            Write(sheet, 4, ["   "]);
            Write(sheet, 5, ["AMD"]);
            sheet.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.White;
        });

        var manufacturerRows = await _sut.ParseManufacturerImportAsync(manufacturers, CancellationToken.None);
        manufacturerRows.Select(x => x.Name).Should().Equal("Intel", "AMD");

        await using var chipsets = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Chipsets");
            Write(sheet, 1, ["Name", "ManufacturerId", "SocketId"]);
            Write(sheet, 2, ["B650", _id.ToString(), _id.ToString()]);
            Write(sheet, 3, ["", "", ""]);
            Write(sheet, 4, ["X870", _id.ToString(), _id.ToString()]);
        });

        var chipsetRows = await _sut.ParseChipsetImportAsync(chipsets, CancellationToken.None);
        chipsetRows.Select(x => x.Name).Should().Equal("B650", "X870");
    }

    [Fact]
    public async Task Invalid_guid_becomes_empty_and_missing_sheet_throws()
    {
        await using var sockets = Workbook(wb =>
        {
            var sheet = wb.Worksheets.Add("Sockets");
            Write(sheet, 1, ["Name", "ManufacturerId"]);
            Write(sheet, 2, ["AM5", "not-a-guid"]);
        });
        (await _sut.ParseSocketImportAsync(sockets, CancellationToken.None))[0]
            .ManufacturerId.Should().Be(Guid.Empty);

        await using var missing = Workbook(wb => wb.Worksheets.Add("Other"));
        var act = () => _sut.ParseManufacturerImportAsync(missing, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }

    private static MemoryStream Workbook(Action<XLWorkbook> configure)
    {
        using var workbook = new XLWorkbook();
        configure(workbook);
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    private static void Write(IXLWorksheet sheet, int row, IReadOnlyList<object> values)
    {
        for (var i = 0; i < values.Count; i++)
        {
            sheet.Cell(row, i + 1).Value = values[i] switch
            {
                int n => n,
                double d => d,
                decimal m => m,
                bool b => b,
                string s => s,
                _ => values[i]?.ToString() ?? string.Empty
            };
        }
    }
}
