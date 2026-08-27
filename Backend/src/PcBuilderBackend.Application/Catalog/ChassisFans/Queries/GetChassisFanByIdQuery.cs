using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public record GetChassisFanByIdQuery(Guid Id) : IRequest<ChassisFanDto?>;