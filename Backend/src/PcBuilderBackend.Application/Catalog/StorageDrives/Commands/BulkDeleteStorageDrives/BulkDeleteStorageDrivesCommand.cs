using MediatR;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkDeleteStorageDrives;

public record BulkDeleteStorageDrivesCommand(List<Guid> Ids) : IRequest<bool>;
