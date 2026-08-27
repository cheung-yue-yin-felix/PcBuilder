using MediatR;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.DeleteStorageDrive;

public record DeleteStorageDriveCommand(Guid Id) : IRequest<bool>;
