using Microsoft.Extensions.Logging;

namespace PcBuilderBackend.Application.Common.Logging;

internal static partial class RequestPipelineLog
{
    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Debug,
        Message = "Handling {RequestName}")]
    public static partial void Handling(ILogger logger, string requestName);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Debug,
        Message = "Handled {RequestName} in {ElapsedMilliseconds}ms")]
    public static partial void Handled(ILogger logger, string requestName, long elapsedMilliseconds);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Error,
        Message = "Error handling {RequestName} after {ElapsedMilliseconds}ms")]
    public static partial void Failed(
        ILogger logger,
        Exception exception,
        string requestName,
        long elapsedMilliseconds);
}
