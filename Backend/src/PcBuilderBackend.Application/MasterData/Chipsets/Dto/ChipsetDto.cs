namespace PcBuilderBackend.Application.MasterData.Chipsets.Dto;

public record ChipsetDto(Guid Id, string Name, Guid ManufacturerId, Guid SocketId);