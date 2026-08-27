using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.ImportPsus;

public record ImportPsusCommand(Stream Stream) : IRequest<List<PsuDto>>;
