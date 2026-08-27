using MediatR;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkDeleteWirelessNetworkAdapters;

public record BulkDeleteWirelessNetworkAdaptersCommand(List<Guid> Ids) : IRequest<bool>;
