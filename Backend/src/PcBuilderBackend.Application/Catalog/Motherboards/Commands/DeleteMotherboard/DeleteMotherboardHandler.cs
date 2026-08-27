using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.DeleteMotherboard;

public class DeleteMotherboardHandler(IUnitOfWork unitOfWork, IMotherboardRepository motherboards) : IRequestHandler<DeleteMotherboardCommand, bool>
{
    public async Task<bool> Handle(DeleteMotherboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await motherboards.GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null || !entity.IsActive) return false;

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
