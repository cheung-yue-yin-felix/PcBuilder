using MediatR;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkDeleteWiredNetworkAdapters;

public record BulkDeleteWiredNetworkAdaptersCommand(List<Guid> Ids) : IRequest<bool>;
