using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        RequestPipelineLog.Handling(logger, requestName);

        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();

            RequestPipelineLog.Handled(logger, requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            RequestPipelineLog.Failed(logger, ex, requestName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
