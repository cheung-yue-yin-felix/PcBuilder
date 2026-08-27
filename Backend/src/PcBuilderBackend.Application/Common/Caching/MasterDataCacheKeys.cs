namespace PcBuilderBackend.Application.Common.Caching;

/// <summary>
/// Cache key conventions for master-data entities.
/// </summary>
public static class MasterDataCacheKeys
{
    public const string Root = "master-data";

    /// <summary>Default TTL for master-data entries.</summary>
    public static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(1);

    public static class Manufacturers
    {
        public const string Prefix = $"{Root}:manufacturers:";
        public static string All() => $"{Prefix}all";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
    }

    public static class Chipsets
    {
        public const string Prefix = $"{Root}:chipsets:";
        public static string All() => $"{Prefix}all";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
    }

    public static class Sockets
    {
        public const string Prefix = $"{Root}:sockets:";
        public static string All() => $"{Prefix}all";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
    }

    public static class CpuSeries
    {
        public const string Prefix = $"{Root}:cpu-series:";
        public static string All() => $"{Prefix}all";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
    }

    public static class GpuSeries
    {
        public const string Prefix = $"{Root}:gpu-series:";
        public static string All() => $"{Prefix}all";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
    }

    public static class Gpus
    {
        public const string Prefix = $"{Root}:gpus:";
        public static string ById(Guid id) => $"{Prefix}{id:D}";
        public static string All() => $"{Prefix}all";
    }
}
