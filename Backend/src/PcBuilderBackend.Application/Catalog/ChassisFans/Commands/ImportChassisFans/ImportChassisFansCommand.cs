using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.ImportChassisFans;

public record ImportChassisFansCommand(Stream Stream) : IRequest<List<ChassisFanDto>>;
