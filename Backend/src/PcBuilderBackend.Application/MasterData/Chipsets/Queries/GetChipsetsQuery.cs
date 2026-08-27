using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public record GetChipsetsQuery(): IRequest<List<ChipsetDto>>;