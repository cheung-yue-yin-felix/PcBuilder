using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;

public class DeleteCpuHandler(IApplicationDbContext context): IRequestHandler<DeleteCpuCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = context.Cpus.FirstOrDefault(c => c.Id == request.Id && c.IsActive);
        if (entity == null) return false;
        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}