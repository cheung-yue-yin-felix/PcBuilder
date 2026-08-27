using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public record GetPsuCablesByPsuIdQuery(Guid PsuId) : IRequest<List<PsuCableDto>>;
