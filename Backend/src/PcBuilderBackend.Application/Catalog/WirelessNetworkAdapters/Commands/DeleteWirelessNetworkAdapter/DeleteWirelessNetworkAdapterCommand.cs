using MediatR;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.DeleteWirelessNetworkAdapter;

public record DeleteWirelessNetworkAdapterCommand(Guid Id) : IRequest<bool>;
