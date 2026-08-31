namespace PcBuilderBackend.Application.MasterData.Sockets.Dto;

public class SocketImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
}
