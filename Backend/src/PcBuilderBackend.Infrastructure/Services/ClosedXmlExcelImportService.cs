using ClosedXML.Excel;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
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

    public Task<List<ChassisImportRow>> ParseChassisImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);

        var chassis = ParseChassis(workbook.Worksheet("Chassis"));
        var driveBays = ParseChassisDriveBays(workbook.Worksheet("ChassisDriveBays"));
        var fanMounts = ParseChassisFanMounts(workbook.Worksheet("ChassisFanMounts"));
        var fanMountOptions = ParseChassisFanMountOptions(workbook.Worksheet("ChassisFanMountOptions"));
        var pcieSlots = ParseChassisPcieSlots(workbook.Worksheet("ChassisPcieSlots"));
        var radiators = ParseChassisRadiators(workbook.Worksheet("ChassisRadiators"));
        var mbFormFactors = ParseChassisMbFormFactors(workbook.Worksheet("ChassisMbFormFactors"));
        var psuFormFactors = ParseChassisPsuFormFactors(workbook.Worksheet("ChassisPsuFormFactors"));

        var chassisByRow = chassis.ToDictionary(x => x.RowNumber);
        var fanMountByRow = fanMounts.ToDictionary(x => x.RowNumber);

        foreach (var bay in driveBays)
        {
            if (chassisByRow.TryGetValue(bay.ParentRowNumber, out var parent))
                parent.DriveBays.Add(bay);
        }

        foreach (var mount in fanMounts)
        {
            if (chassisByRow.TryGetValue(mount.ParentRowNumber, out var parent))
                parent.FanMounts.Add(mount);
        }

        foreach (var option in fanMountOptions)
        {
            if (fanMountByRow.TryGetValue(option.ParentRowNumber, out var mount))
                mount.Options.Add(option);
        }

        foreach (var slot in pcieSlots)
        {
            if (chassisByRow.TryGetValue(slot.ParentRowNumber, out var parent))
                parent.PcieSlots.Add(slot);
        }

        foreach (var radiator in radiators)
        {
            if (chassisByRow.TryGetValue(radiator.ParentRowNumber, out var parent))
                parent.Radiators.Add(radiator);
        }

        foreach (var formFactor in mbFormFactors)
        {
            if (chassisByRow.TryGetValue(formFactor.ParentRowNumber, out var parent))
                parent.MbFormFactors.Add(formFactor);
        }

        foreach (var formFactor in psuFormFactors)
        {
            if (chassisByRow.TryGetValue(formFactor.ParentRowNumber, out var parent))
                parent.PsuFormFactors.Add(formFactor);
        }

        return Task.FromResult(chassis);
    }

    public Task<List<CpuCoolerImportRow>> ParseCpuCoolerImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);

        var coolers = ParseCpuCoolers(workbook.Worksheet("CpuCoolers"));
        var sockets = ParseCpuCoolerSockets(workbook.Worksheet("CpuCoolerSockets"));

        var coolerByRow = coolers.ToDictionary(x => x.RowNumber);
        foreach (var socket in sockets)
        {
            if (coolerByRow.TryGetValue(socket.ParentRowNumber, out var cooler))
                cooler.Sockets.Add(socket);
        }

        return Task.FromResult(coolers);
    }

    public Task<List<GraphicsCardImportRow>> ParseGraphicsCardImportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var cards = ParseGraphicsCards(workbook.Worksheet("GraphicsCards"));
        return Task.FromResult(cards);
    }

    public Task<List<ChassisFanImportRow>> ParseChassisFanImportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var fans = ParseChassisFans(workbook.Worksheet("ChassisFans"));
        return Task.FromResult(fans);
    }

    public Task<List<PsuImportRow>> ParsePsuImportAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);

        var psus = ParsePsus(workbook.Worksheet("Psus"));
        var cables = workbook.TryGetWorksheet("PsuCables", out var cableSheet)
            ? ParsePsuCables(cableSheet)
            : [];

        var psuByRow = psus.ToDictionary(x => x.RowNumber);
        foreach (var cable in cables)
        {
            if (psuByRow.TryGetValue(cable.ParentRowNumber, out var psu))
                psu.Cables.Add(cable);
        }

        return Task.FromResult(psus);
    }

    public Task<List<StorageDriveImportRow>> ParseStorageDriveImportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var drives = ParseStorageDrives(workbook.Worksheet("StorageDrives"));
        return Task.FromResult(drives);
    }

    public Task<List<WiredNetworkAdapterImportRow>> ParseWiredNetworkAdapterImportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var adapters = ParseWiredNetworkAdapters(workbook.Worksheet("WiredNetworkAdapters"));
        return Task.FromResult(adapters);
    }

    public Task<List<WirelessNetworkAdapterImportRow>> ParseWirelessNetworkAdapterImportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(stream);
        var adapters = ParseWirelessNetworkAdapters(workbook.Worksheet("WirelessNetworkAdapters"));
        return Task.FromResult(adapters);
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

    private static List<ChassisImportRow> ParseChassis(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    LengthMm = (decimal)row.Cell(3).GetDouble(),
                    WidthMm = (decimal)row.Cell(4).GetDouble(),
                    HeightMm = (decimal)row.Cell(5).GetDouble(),
                    MotherboardMaxWidthMm = (decimal)row.Cell(6).GetDouble(),
                    MotherboardMaxHeightMm = (decimal)row.Cell(7).GetDouble(),
                    MaxCpuCoolerHeightMm = (decimal)row.Cell(8).GetDouble(),
                    MaxGraphicsCardLengthMm = (decimal)row.Cell(9).GetDouble(),
                    MaxPsuLengthMm = (decimal)row.Cell(10).GetDouble()
                })
        ];
    }

    private static List<ChassisDriveBayImportRow> ParseChassisDriveBays(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisDriveBayImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    FormFactor = Enum.Parse<DriveBayFormFactor>(row.Cell(2).GetString()),
                    SlotCount = (int)row.Cell(3).GetDouble()
                })
        ];
    }

    private static List<ChassisFanMountImportRow> ParseChassisFanMounts(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisFanMountImportRow
                {
                    RowNumber = row.RowNumber(),
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    Location = Enum.Parse<FanMountLocation>(row.Cell(2).GetString()),
                    SingleDiameterOnly = row.Cell(3).GetBoolean()
                })
        ];
    }

    private static List<ChassisFanMountOptionImportRow> ParseChassisFanMountOptions(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisFanMountOptionImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    Diameter = Enum.Parse<FanDiameterMm>(row.Cell(2).GetString()),
                    SlotCount = (int)row.Cell(3).GetDouble()
                })
        ];
    }

    private static List<ChassisPcieSlotImportRow> ParseChassisPcieSlots(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisPcieSlotImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    LowProfileSlots = row.Cell(2).GetBoolean(),
                    SlotCount = (int)row.Cell(3).GetDouble(),
                    Orientation = Enum.Parse<PcieOrientation>(row.Cell(4).GetString())
                })
        ];
    }

    private static List<ChassisRadiatorImportRow> ParseChassisRadiators(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisRadiatorImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    Length = Enum.Parse<RadiatorLength>(row.Cell(2).GetString()),
                    Location = Enum.Parse<RadiatorMountLocation>(row.Cell(3).GetString()),
                    RadiatorCount = (int)row.Cell(4).GetDouble()
                })
        ];
    }

    private static List<ChassisMbFormFactorImportRow> ParseChassisMbFormFactors(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisMbFormFactorImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    MbFormFactor = Enum.Parse<MbFormFactor>(row.Cell(2).GetString())
                })
        ];
    }

    private static List<ChassisPsuFormFactorImportRow> ParseChassisPsuFormFactors(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisPsuFormFactorImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    PsuFormFactor = Enum.Parse<PsuFormFactor>(row.Cell(2).GetString())
                })
        ];
    }

    private static List<CpuCoolerImportRow> ParseCpuCoolers(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new CpuCoolerImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    MaxTdp = (int)row.Cell(3).GetDouble(),
                    Type = Enum.Parse<CpuCoolerType>(row.Cell(4).GetString()),
                    CoolerHeightMm = ParseOptionalDecimal(row.Cell(5)),
                    MaxRamHeightMm = ParseOptionalDecimal(row.Cell(6)),
                    RadiatorLength = ParseOptionalEnum<RadiatorLength>(row.Cell(7))
                })
        ];
    }

    private static List<CpuCoolerSocketImportRow> ParseCpuCoolerSockets(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new CpuCoolerSocketImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    SocketId = Guid.TryParse(row.Cell(2).GetString(), out var socketId)
                        ? socketId
                        : Guid.Empty
                })
        ];
    }

    private static List<GraphicsCardImportRow> ParseGraphicsCards(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new GraphicsCardImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    GpuId = Guid.TryParse(row.Cell(3).GetString(), out var gpuId) ? gpuId : Guid.Empty,
                    VideoMemoryGb = (int)row.Cell(4).GetDouble(),
                    PcieSlotsUsed = (int)row.Cell(5).GetDouble(),
                    PcieGeneration = Enum.Parse<PcieGeneration>(row.Cell(6).GetString()),
                    LengthMm = (decimal)row.Cell(7).GetDouble(),
                    WidthMm = (decimal)row.Cell(8).GetDouble(),
                    HeightMm = (decimal)row.Cell(9).GetDouble(),
                    PowerConsumptionWatts = (int)row.Cell(10).GetDouble(),
                    PowerConnectorType = Enum.Parse<PsuCableType>(row.Cell(11).GetString()),
                    PowerConnectorCount = (int)row.Cell(12).GetDouble()
                })
        ];
    }

    private static List<ChassisFanImportRow> ParseChassisFans(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new ChassisFanImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    DiameterMm = Enum.Parse<FanDiameterMm>(row.Cell(3).GetString()),
                    FansCountPerPack = (int)row.Cell(4).GetDouble()
                })
        ];
    }

    private static List<PsuImportRow> ParsePsus(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new PsuImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    Wattage = (int)row.Cell(3).GetDouble(),
                    Modularity = Enum.Parse<PsuModularity>(row.Cell(4).GetString()),
                    FormFactor = Enum.Parse<PsuFormFactor>(row.Cell(5).GetString()),
                    LengthMm = (decimal)row.Cell(6).GetDouble(),
                    WidthMm = (decimal)row.Cell(7).GetDouble(),
                    HeightMm = (decimal)row.Cell(8).GetDouble()
                })
        ];
    }

    private static List<PsuCableImportRow> ParsePsuCables(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new PsuCableImportRow
                {
                    ParentRowNumber = (int)row.Cell(1).GetDouble(),
                    Type = Enum.Parse<PsuCableType>(row.Cell(2).GetString()),
                    CablesCount = (int)row.Cell(3).GetDouble(),
                    ConnectorsCount = (int)row.Cell(4).GetDouble()
                })
        ];
    }

    private static List<StorageDriveImportRow> ParseStorageDrives(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new StorageDriveImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    Media = Enum.Parse<StorageMedia>(row.Cell(3).GetString()),
                    Interface = Enum.Parse<StorageInterface>(row.Cell(4).GetString()),
                    FormFactor = Enum.Parse<StorageFormFactor>(row.Cell(5).GetString()),
                    CapacityGb = (int)row.Cell(6).GetDouble(),
                    PcieGeneration = ParseOptionalEnum<PcieGeneration>(row.Cell(7)),
                    Rpm = ParseOptionalInt(row.Cell(8))
                })
        ];
    }

    private static List<WiredNetworkAdapterImportRow> ParseWiredNetworkAdapters(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new WiredNetworkAdapterImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    HostInterface = Enum.Parse<WiredHostInterface>(row.Cell(3).GetString()),
                    MaxSpeedMbps = (int)row.Cell(4).GetDouble(),
                    UsbVersion = ParseOptionalEnum<UsbVersion>(row.Cell(5)),
                    UsbType = ParseOptionalEnum<UsbType>(row.Cell(6)),
                    PcieSlotType = ParseOptionalEnum<PcieSlotType>(row.Cell(7))
                })
        ];
    }

    private static List<WirelessNetworkAdapterImportRow> ParseWirelessNetworkAdapters(IXLWorksheet sheet)
    {
        return
        [
            .. sheet.RowsUsed()
                .Skip(1)
                .Select(row => new WirelessNetworkAdapterImportRow
                {
                    RowNumber = row.RowNumber(),
                    Name = row.Cell(1).GetString(),
                    ManufacturerId = Guid.TryParse(row.Cell(2).GetString(), out var manufacturerId)
                        ? manufacturerId
                        : Guid.Empty,
                    WifiStandard = Enum.Parse<WifiStandard>(row.Cell(3).GetString()),
                    HostInterface = Enum.Parse<WirelessHostInterface>(row.Cell(4).GetString()),
                    MaxSpeedMbps = (int)row.Cell(5).GetDouble(),
                    MaxSpeedMbps5G = ParseOptionalInt(row.Cell(6)),
                    MaxSpeedMbps6G = ParseOptionalInt(row.Cell(7)),
                    BluetoothVersion = ParseOptionalEnum<BluetoothVersion>(row.Cell(8)),
                    PcieSlotType = ParseOptionalEnum<PcieSlotType>(row.Cell(9)),
                    Key = ParseOptionalEnum<M2Key>(row.Cell(10)),
                    M2FormFactor = ParseOptionalEnum<M2FormFactor>(row.Cell(11)),
                    UsbVersion = ParseOptionalEnum<UsbVersion>(row.Cell(12)),
                    UsbType = ParseOptionalEnum<UsbType>(row.Cell(13))
                })
        ];
    }

    private static decimal? ParseOptionalDecimal(IXLCell cell)
    {
        return cell.IsEmpty() || string.IsNullOrWhiteSpace(cell.GetString())
            ? null
            : (decimal)cell.GetDouble();
    }

    private static int? ParseOptionalInt(IXLCell cell)
    {
        return cell.IsEmpty() || string.IsNullOrWhiteSpace(cell.GetString())
            ? null
            : (int)cell.GetDouble();
    }

    private static TEnum? ParseOptionalEnum<TEnum>(IXLCell cell)
        where TEnum : struct, Enum
    {
        if (cell.IsEmpty())
            return null;

        var value = cell.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : Enum.Parse<TEnum>(value);
    }
}