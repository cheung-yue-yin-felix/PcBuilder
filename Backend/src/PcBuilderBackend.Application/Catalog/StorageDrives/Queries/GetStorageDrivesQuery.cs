using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public record GetStorageDrivesQuery(PagedRequest Request) : IRequest<PagedResult<StorageDriveDto>>;
