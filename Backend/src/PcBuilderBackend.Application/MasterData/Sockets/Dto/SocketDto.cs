namespace PcBuilderBackend.Application.MasterData.Sockets.Dto;

public record SocketDto(
    Guid Id, 
    Guid ManufacturerId, 
    string ManufacturerName,
    string Name
);