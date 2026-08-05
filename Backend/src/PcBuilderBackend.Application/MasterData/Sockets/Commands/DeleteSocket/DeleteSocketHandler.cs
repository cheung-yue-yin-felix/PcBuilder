using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;

public class DeleteSocketHandler(IApplicationDbContext context): IRequestHandler<DeleteSocketCommand, bool>
{
    public async Task<bool> Handle(DeleteSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Sockets.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);
        if (entity is null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}