namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IActiveEntityLookup
{
    Task<bool> ManufacturerExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> SocketExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ChipsetExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> CpuSeriesExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> GpuSeriesExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> GpuExistsAsync(Guid id, CancellationToken cancellationToken);

    Task<int> CountManufacturersAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<int> CountSocketsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<int> CountChipsetsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<int> CountCpuSeriesAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<int> CountGpuSeriesAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<int> CountGpusAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
}
