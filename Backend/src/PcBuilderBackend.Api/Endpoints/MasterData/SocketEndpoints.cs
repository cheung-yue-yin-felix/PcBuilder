using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkUpdateSockets;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkDeleteSockets;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.ImportSockets;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Application.MasterData.Sockets.Queries;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class SocketEndpoints
{
    public static void MapSocketEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("socket")
            .WithSidebarGroup("MasterData", "Socket")
            .WithDescription("Browse, Read, Edit, Add and Delete Sockets");
        
        subgroup.MapGet("/", GetSockets)
            .Produces<List<SocketDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse Sockets");
        
        subgroup.MapGet("/{id}", GetSocketById)
            .Produces<SocketDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read a Socket by ID");
        
        subgroup.MapPut("/", UpdateSocket)
            .Produces<SocketDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a Socket");

        subgroup.MapPost("/", CreateSocket)
            .Produces<SocketDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new Socket");
        
        subgroup.MapDelete("/{id}", DeleteSocket)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Socket");
        
        subgroup.MapPost("/bulk", BulkCreateSockets)
            .Produces<List<SocketDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple new Sockets");
        
        subgroup.MapPut("/bulk", BulkUpdateSockets)
            .Produces<List<SocketDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple Sockets");
        
        subgroup.MapDelete("/bulk", BulkDeleteSockets)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple Sockets");

        subgroup.MapPost("/import", ImportSockets)
            .Produces<List<SocketDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import Sockets from Excel")
            .WithDescription("\n    POST /master-data/socket/import");
    }

    private static async Task<Ok<List<SocketDto>>> GetSockets(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetSocketsQuery(), cancellationToken));
    }
    
    private static async Task<Results<Ok<SocketDto>, NotFound>> GetSocketById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var socket = await sender.Send(new GetSocketByIdQuery(id), cancellationToken);
        return socket is null ? TypedResults.NotFound() : TypedResults.Ok(socket);
    }
    
    private static async Task<Results<Ok<SocketDto>, NotFound>> UpdateSocket(
        [FromBody] UpdateSocketCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var updatedSocket = await sender.Send(command, cancellationToken);
        return updatedSocket is not null ? TypedResults.Ok(updatedSocket) : TypedResults.NotFound();
    }

    private static async Task<Created<SocketDto>> CreateSocket(
        [Validate] [FromBody] CreateSocketCommand command,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{context.Request.Path}/{result.Id}", result);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteSocket(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSocketCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Created<List<SocketDto>>> BulkCreateSockets(
        [Validate] [FromBody] BulkCreateSocketsCommand command,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created(context.Request.Path, result);
    }

    private static async Task<Results<Ok<List<SocketDto>>, NotFound>> BulkUpdateSockets(
        [FromBody] BulkUpdateSocketsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteSockets(
        [FromBody] BulkDeleteSocketsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<SocketDto>>> ImportSockets(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        return TypedResults.Ok(await sender.Send(new ImportSocketsCommand(fileStream), cancellationToken));
    }
}
