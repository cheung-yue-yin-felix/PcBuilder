using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Build.Queries;

namespace PcBuilderBackend.Api.Endpoints.Build;

public static class PcBuildEndpoints
{
    public static void MapPcBuildEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/builds")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithSidebarGroup("Builds", "PC Build")
            .WithDescription("Check and persist PC builds");

        group.MapPost("/compatibility", CheckCompatibility)
            .AllowAnonymous()
            .Produces<CompatibilityCheckDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Check draft compatibility")
            .WithDescription("\n    POST /api/builds/compatibility");
    }

    private static async Task<Ok<CompatibilityCheckDto>> CheckCompatibility(
        [FromBody] CheckPcBuildCompatibilityQuery query,
        [FromServices] ISender sender,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await sender.Send(query, cancellationToken));
}
