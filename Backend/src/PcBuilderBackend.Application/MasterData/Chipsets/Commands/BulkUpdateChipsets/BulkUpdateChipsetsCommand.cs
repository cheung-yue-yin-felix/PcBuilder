using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkUpdateChipsets;

public record BulkUpdateChipsetsCommand(List<ChipsetDto> Chipsets) : IRequest<List<ChipsetDto>?>;

