using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using MediatR;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;

public record BulkCreateChipsetsCommand(List<ChipsetDto> Chipsets) : IRequest<List<ChipsetDto>>;