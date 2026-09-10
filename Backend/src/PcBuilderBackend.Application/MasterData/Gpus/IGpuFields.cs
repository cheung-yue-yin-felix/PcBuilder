namespace PcBuilderBackend.Application.MasterData.Gpus;

public interface IGpuFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    Guid GpuSeriesId { get; }
}
