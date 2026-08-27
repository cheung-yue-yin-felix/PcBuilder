using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.ImportStorageDrives;

public record ImportStorageDrivesCommand(Stream Stream) : IRequest<List<StorageDriveDto>>;
