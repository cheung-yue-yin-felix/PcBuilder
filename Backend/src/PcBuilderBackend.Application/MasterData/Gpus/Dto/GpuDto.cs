namespace PcBuilderBackend.Application.MasterData.Gpus.Dto;

public record GpuDto(Guid Id, Guid ManufacturerId, Guid GpuSeriesId, string Name);