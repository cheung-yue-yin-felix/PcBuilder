using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsuCables;

public record BulkUpdatePsuCablesCommand(
    Guid PsuId,
    List<PsuCableDto> Cables) : IRequest<List<PsuCableDto>?>;
