using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.DeletePsu;

public class DeletePsuHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    ILogger<DeletePsuHandler> logger)
    : IRequestHandler<DeletePsuCommand, bool>
{
    public async Task<bool> Handle(DeletePsuCommand request, CancellationToken cancellationToken)
    {
        var entity = await psus.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Psu, request.Id);
            return false;
        }

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Deleted(logger, EntityLog.Psu, entity.Id);
        return true;
    }
}
