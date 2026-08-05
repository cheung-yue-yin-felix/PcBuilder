using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

public record CreateChipsetCommand(string Name, Guid ManufacturerId, Guid SocketId): IRequest<ChipsetDto>;