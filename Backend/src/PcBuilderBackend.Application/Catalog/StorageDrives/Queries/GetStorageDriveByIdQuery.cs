using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public record GetStorageDriveByIdQuery(Guid Id) : IRequest<StorageDriveDto?>;
