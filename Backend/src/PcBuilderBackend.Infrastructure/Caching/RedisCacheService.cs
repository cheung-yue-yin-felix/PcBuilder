using System.Text.Json;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using StackExchange.Redis;

namespace PcBuilderBackend.Infrastructure.Caching;

public sealed class RedisCacheService(
    IConnectionMultiplexer connectionMultiplexer,
    ILogger<RedisCacheService> logger) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly IDatabase _db = connectionMultiplexer.GetDatabase();
    private readonly IConnectionMultiplexer _mux = connectionMultiplexer;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(value.ToString(), JsonOptions);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to deserialize cache key {CacheKey}; removing entry.", key);
            await _db.KeyDeleteAsync(key);
            return default;
        }
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(value, JsonOptions);
        await _db.StringSetAsync(key, payload, absoluteExpiration);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => _db.KeyDeleteAsync(key);

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        foreach (var endpoint in _mux.GetEndPoints())
        {
            var server = _mux.GetServer(endpoint);
            if (server is null || !server.IsConnected || server.IsReplica)
                continue;

            var keys = server.KeysAsync(pattern: $"{prefix}*");
            var batch = new List<RedisKey>(64);

            await foreach (var key in keys.WithCancellation(cancellationToken))
            {
                batch.Add(key);
                if (batch.Count < 64)
                    continue;

                await _db.KeyDeleteAsync(batch.ToArray());
                batch.Clear();
            }

            if (batch.Count > 0)
                await _db.KeyDeleteAsync(batch.ToArray());
        }
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default)
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var value = await factory(cancellationToken);

        // Do not cache null reference types as successful hits.
        if (value is not null)
            await SetAsync(key, value, absoluteExpiration, cancellationToken);

        return value;
    }
}
