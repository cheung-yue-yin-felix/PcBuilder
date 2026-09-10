namespace PcBuilderBackend.Application.MasterData.Chipsets;

public interface IChipsetFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    Guid SocketId { get; }
}
