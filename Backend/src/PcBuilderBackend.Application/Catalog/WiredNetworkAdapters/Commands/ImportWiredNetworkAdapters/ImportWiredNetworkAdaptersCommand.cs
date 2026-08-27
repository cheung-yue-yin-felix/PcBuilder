using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.ImportWiredNetworkAdapters;

public record ImportWiredNetworkAdaptersCommand(Stream Stream) : IRequest<List<WiredNetworkAdapterDto>>;
