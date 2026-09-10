namespace PcBuilderBackend.Application.MasterData.CpuSeries;

public interface ICpuSeriesFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    Guid SocketId { get; }
}
