using MediatR;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.DeleteWiredNetworkAdapter;

public record DeleteWiredNetworkAdapterCommand(Guid Id) : IRequest<bool>;
