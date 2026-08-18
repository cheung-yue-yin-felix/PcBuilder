using ClosedXML.Excel;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Services;

public class ClosedXmlExcelImportService : IExcelImportService
{
    public Task<List<CpuImportRow>> ParseCpuImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);

        var cpus = ParseCpus(workbook.Worksheet("CPUs"));
        var compats = ParseRamCompats(workbook.Worksheet("CpuRamCompats"));
        var supportChipsets = ParseSupportChipsets(workbook.Worksheet("CpuSupportChipsets"));

        var cpuByRow = cpus.ToDictionary(x => x.RowNumber);
        foreach (var compat in compats)
        {
            if (cpuByRow.TryGetValue(compat.ParentRowNumber, out var cpu))
                cpu.RamCompats.Add(compat);
        }

        foreach (var support in supportChipsets)
        {
            if (cpuByRow.TryGetValue(support.ParentRowNumber, out var cpu))
                cpu.SupportChipsets.Add(support);
        }

        return Task.FromResult(cpus);
    }

    public Task<List<GpuImportRow>> ParseGpuImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var gpus = ParseGpus(workbook.Worksheet("GPUs"));
        return Task.FromResult(gpus);
    }

    public Task<List<RamImportRow>> ParseRamImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var rams = ParseRams(workbook.Worksheet("RAMs"));
        return Task.FromResult(rams);
    }

    public Task<List<MotherboardImportRow>> ParseMotherboardImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);

        var motherboards = ParseMotherboards(workbook.Worksheet("Motherboards"));
        var pcieSlots = ParseMotherboardPcieSlots(workbook.Worksheet("MotherboardPcieSlots"));
        var m2Slots = ParseMotherboardM2Slots(workbook.Worksheet("MotherboardM2Slots"));
        var usbPorts = ParseMotherboardUsbPorts(workbook.Worksheet("MotherboardUsbPorts"));

        var motherboardByRow = motherboards.ToDictionary(x => x.RowNumber);

        foreach (var slot in pcieSlots)
        {
            if (motherboardByRow.TryGetValue(slot.ParentRowNumber, out var motherboard))
                motherboard.PcieSlots.Add(slot);
        }

        foreach (var slot in m2Slots)
        {
            if (motherboardByRow.TryGetValue(slot.ParentRowNumber, out var motherboard))
                motherboard.M2Slots.Add(slot);
        }

        foreach (var port in usbPorts)
        {
            if (motherboardByRow.TryGetValue(port.ParentRowNumber, out var motherboard))
                motherboard.UsbPorts.Add(port);
        }

        return Task.FromResult(motherboards);
    }

    private static List<CpuImportRow> ParseCpus(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new CpuImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    SocketId = Guid.TryParse(row.Cell(3).GetString(), out var socketId) ? socketId : Guid.Empty,
                    SeriesId = Guid.TryParse(row.Cell(4).GetString(), out var seriesId) ? seriesId : Guid.Empty,
                    MaxMemoryGb = (int)row.Cell(5).GetDouble(),
                    IntegratedGraphics = row.Cell(6).GetBoolean(),
                    IncludedStockCooler = row.Cell(7).GetBoolean(),
                    ThermalDesignPower = (int)row.Cell(8).GetDouble(),
                    PowerConsumptionWatts = (int)row.Cell(9).GetDouble()
                })
        ];
    }

    private static List<CpuRamCompactImportRow> ParseRamCompats(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new CpuRamCompactImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    DdrGeneration = Enum.Parse<DdrGeneration>(row.Cell(2).GetString()),
                    RamModuleCount = (int)row.Cell(3).GetDouble(),
                    RamRank = Enum.Parse<RamRank>(row.Cell(4).GetString()),
                    MaxSpeedMts = (int)row.Cell(5).GetDouble()
                })
        ];
    }

    private static List<CpuSupportChipsetImportRow> ParseSupportChipsets(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new CpuSupportChipsetImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    ChipsetId = Guid.TryParse(row.Cell(2).GetString(), out var chipsetId)
                        ? chipsetId
                        : Guid.Empty,
                    RequiresBiosUpdate = row.Cell(3).GetBoolean()
                })
        ];
    }

    private static List<GpuImportRow> ParseGpus(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new GpuImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    SeriesId = Guid.TryParse(row.Cell(3).GetString(), out var seriesId) ? seriesId : Guid.Empty
                })
        ];
    }

    private static List<RamImportRow> ParseRams(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new RamImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    Color = row.Cell(3).GetString(),
                    DdrGeneration = Enum.Parse<DdrGeneration>(row.Cell(4).GetString()),
                    RamFormFactor = Enum.Parse<RamFormFactor>(row.Cell(5).GetString()),
                    RamRank = Enum.Parse<RamRank>(row.Cell(6).GetString()),
                    MemorySizePerStickGb = (int)row.Cell(7).GetDouble(),
                    TotalMemorySizeGb = (int)row.Cell(8).GetDouble(),
                    ModulesCount = (int)row.Cell(9).GetDouble(),
                    MaxMemorySpeedMts = (int)row.Cell(10).GetDouble(),
                    HeightMm = (decimal)row.Cell(11).GetDouble()
                })
        ];
    }

    private static List<MotherboardImportRow> ParseMotherboards(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new MotherboardImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    SocketId = Guid.TryParse(row.Cell(3).GetString(), out var socketId) ? socketId : Guid.Empty,
                    ChipsetId = Guid.TryParse(row.Cell(4).GetString(), out var chipsetId) ? chipsetId : Guid.Empty,
                    RamSlots = (int)row.Cell(5).GetDouble(),
                    MaxMemoryGb = (int)row.Cell(6).GetDouble(),
                    MaxDimmSizeGb = (int)row.Cell(7).GetDouble(),
                    SataPorts = (int)row.Cell(8).GetDouble(),
                    FanConnectors = (int)row.Cell(9).GetDouble(),
                    EpsConnectors = (int)row.Cell(10).GetDouble(),
                    WidthMm = (decimal)row.Cell(11).GetDouble(),
                    HeightMm = (decimal)row.Cell(12).GetDouble(),
                    DdrGeneration = Enum.Parse<DdrGeneration>(row.Cell(13).GetString()),
                    RamFormFactor = Enum.Parse<RamFormFactor>(row.Cell(14).GetString()),
                    FormFactor = Enum.Parse<MbFormFactor>(row.Cell(15).GetString()),
                    WifiEnabled = row.Cell(16).GetBoolean(),
                    BluetoothEnabled = row.Cell(17).GetBoolean()
                })
        ];
    }

    private static List<MotherboardPcieImportRow> ParseMotherboardPcieSlots(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new MotherboardPcieImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    SlotType = Enum.Parse<PcieSlotType>(row.Cell(2).GetString()),
                    SlotLanes = Enum.Parse<PcieSlotLane>(row.Cell(3).GetString()),
                    Generation = Enum.Parse<PcieGeneration>(row.Cell(4).GetString()),
                    SlotCount = (int)row.Cell(5).GetDouble()
                })
        ];
    }

    private static List<MotherboardM2ImportRow> ParseMotherboardM2Slots(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new MotherboardM2ImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    PcieGeneration = Enum.Parse<PcieGeneration>(row.Cell(2).GetString()),
                    SlotCount = (int)row.Cell(3).GetDouble(),
                    FormFactors = ParseM2FormFactors(row.Cell(4).GetString()),
                    Key = row.Cell(5).IsEmpty()
                        ? M2Key.M
                        : Enum.Parse<M2Key>(row.Cell(5).GetString()),
                    SupportsSata = !row.Cell(6).IsEmpty() && row.Cell(6).GetBoolean()
                })
        ];
    }

    private static List<MotherboardUsbImportRow> ParseMotherboardUsbPorts(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new MotherboardUsbImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    UsbVersion = Enum.Parse<UsbVersion>(row.Cell(2).GetString()),
                    UsbType = Enum.Parse<UsbType>(row.Cell(3).GetString()),
                    PortCount = (int)row.Cell(4).GetDouble()
                })
        ];
    }

    private static List<M2FormFactor> ParseM2FormFactors(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return [];

        return
        [
            .. value
                .Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(Enum.Parse<M2FormFactor>)
        ];
    }
}