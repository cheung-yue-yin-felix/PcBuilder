using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Commands.UpdatePsu;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsus;

public record BulkUpdatePsusCommand(List<UpdatePsuCommand> Psus) : IRequest<List<PsuDto>?>;
