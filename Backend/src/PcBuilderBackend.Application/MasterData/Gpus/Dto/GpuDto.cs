namespace PcBuilderBackend.Application.MasterData.Gpus.Dto;

public record GpuDto(
    Guid Id, 
    Guid ManufacturerId, 
    string ManufacturerName,
    Guid GpuSeriesId, 
    string GpuSeriesName,
    string Name
);