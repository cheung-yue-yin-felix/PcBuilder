using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Build.Commands.DeletePcBuild;

public class DeletePcBuildHandler(
    IUnitOfWork unitOfWork,
    IPcBuildRepository pcBuilds,
    ICurrentUser currentUser,
    ILogger<DeletePcBuildHandler> logger) : IRequestHandler<DeletePcBuildCommand, bool>
{
    public async Task<bool> Handle(DeletePcBuildCommand command, CancellationToken cancellationToken)
    {
        var pcBuild = await pcBuilds.GetWithChildrenAsync(command.Id);

        if (pcBuild is null)
        {
            EntityLog.NotFound(logger, EntityLog.PcBuild, command.Id);
            return false;
        }

        if (pcBuild.User is null || !currentUser.UserId.HasValue || pcBuild.User.UserId != currentUser.UserId.Value)
        {
            EntityLog.NotAuthorized(logger, EntityLog.PcBuild, command.Id);
            return false;
        }

        pcBuild.Deactivate();

        var deleteUserLog = false;
        var userId = Guid.Empty;

        if (pcBuild.User is not null)
        {
            pcBuild.User.Deactivate();
            deleteUserLog = true;
            userId = pcBuild.User.Id;
        }
    
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.PcBuild, pcBuild.Id);

        if (deleteUserLog)
            EntityLog.Deleted(logger, EntityLog.PcBuildUser, userId);

        return true;
    }
}