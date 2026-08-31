using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Infrastructure.Persistence;

public sealed class ActiveEntityLookup(PcBuilderDbContext db) : IActiveEntityLookup
{
    public Task<bool> ManufacturerExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.Manufacturers.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> SocketExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.Sockets.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ChipsetExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.Chipsets.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> CpuSeriesExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.CpuSeries.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> GpuSeriesExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.GpuSeries.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> GpuExistsAsync(Guid id, CancellationToken cancellationToken) =>
        db.Gpus.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<int> CountManufacturersAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.Manufacturers.CountAsync(x => ids.Contains(x.Id), cancellationToken);

    public Task<int> CountSocketsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.Sockets.CountAsync(x => ids.Contains(x.Id), cancellationToken);

    public Task<int> CountChipsetsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.Chipsets.CountAsync(x => ids.Contains(x.Id), cancellationToken);

    public Task<int> CountCpuSeriesAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.CpuSeries.CountAsync(x => ids.Contains(x.Id), cancellationToken);

    public Task<int> CountGpuSeriesAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.GpuSeries.CountAsync(x => ids.Contains(x.Id), cancellationToken);

    public Task<int> CountGpusAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        db.Gpus.CountAsync(x => ids.Contains(x.Id), cancellationToken);
}
