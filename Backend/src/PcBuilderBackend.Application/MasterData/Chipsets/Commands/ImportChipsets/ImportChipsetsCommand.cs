using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.ImportChipsets;

public record ImportChipsetsCommand(Stream Stream) : IRequest<List<ChipsetDto>>;
