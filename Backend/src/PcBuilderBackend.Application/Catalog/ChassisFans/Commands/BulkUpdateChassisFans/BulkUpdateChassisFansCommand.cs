using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.UpdateChassisFan;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkUpdateChassisFans;

public record BulkUpdateChassisFansCommand(List<UpdateChassisFanCommand> Fans)
    : IRequest<List<ChassisFanDto>?>;
