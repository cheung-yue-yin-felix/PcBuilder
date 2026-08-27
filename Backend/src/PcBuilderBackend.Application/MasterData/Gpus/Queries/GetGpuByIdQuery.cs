using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public record GetGpuByIdQuery(Guid Id) : IRequest<GpuDto?>;