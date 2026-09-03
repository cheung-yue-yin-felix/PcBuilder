using MediatR;
using PcBuilderBackend.Application.Build.Dto;

namespace PcBuilderBackend.Application.Build.Queries;

public record GetPcBuildByIdQuery(Guid Id) : IRequest<PcBuildDto?>;