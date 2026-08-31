namespace PcBuilderBackend.Application.MasterData.Chipsets.Dto;

public class ChipsetImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
}
