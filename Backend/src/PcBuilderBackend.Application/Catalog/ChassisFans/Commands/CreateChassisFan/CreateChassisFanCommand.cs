using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.CreateChassisFan;

public record CreateChassisFanCommand : IRequest<ChassisFanDto>
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public FanDiameterMm DiameterMm { get; init; }
    public int FansCountPerPack { get; init; }
}
