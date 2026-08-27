using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;

public record UpdateChipsetCommand(Guid Id, string Name, Guid ManufacturerId, Guid SocketId): IRequest<ChipsetDto?>;