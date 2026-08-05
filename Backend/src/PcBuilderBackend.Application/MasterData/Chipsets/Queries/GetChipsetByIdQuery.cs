using MediatR;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public record GetChipsetByIdQuery(Guid Id) : IRequest<ChipsetDto?>;