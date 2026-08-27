using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.ImportWirelessNetworkAdapters;

public record ImportWirelessNetworkAdaptersCommand(Stream Stream) : IRequest<List<WirelessNetworkAdapterDto>>;
