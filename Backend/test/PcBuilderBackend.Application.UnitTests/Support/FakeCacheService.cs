using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class FakeCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) =>
        Task.FromResult(default(T));

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) =>
        factory(cancellationToken);
}
