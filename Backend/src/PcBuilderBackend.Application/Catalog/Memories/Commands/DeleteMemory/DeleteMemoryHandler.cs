using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.DeleteMemory;

public class DeleteMemoryHandler(IRamRepository memories, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMemoryCommand, bool>
{
    public async Task<bool> Handle(DeleteMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await memories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
