namespace PcBuilderBackend.Api.Extensions;

public static class SpaFallbackExtensions
{
    public static void MapSpaFallback(this WebApplication app)
    {
        app.MapFallback(async context =>
        {
            if (context.Request.Path.StartsWithSegments("/api")
                || context.Request.Path.StartsWithSegments("/openapi")
                || context.Request.Path.StartsWithSegments("/scalar"))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var file = app.Environment.WebRootFileProvider.GetFileInfo("index.html");
            if (!file.Exists || file.PhysicalPath is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync(file);
        }).ExcludeFromDescription();
    }
}
