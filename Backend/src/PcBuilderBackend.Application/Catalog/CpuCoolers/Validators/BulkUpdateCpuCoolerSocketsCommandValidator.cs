using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkUpdateCpuCoolerSockets;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class BulkUpdateCpuCoolerSocketsCommandValidator
    : AbstractValidator<BulkUpdateCpuCoolerSocketsCommand>
{
    public BulkUpdateCpuCoolerSocketsCommandValidator()
    {
        RuleFor(x => x.CpuCoolerId)
            .NotEmpty().WithMessage("CpuCoolerId is required");

        RuleFor(x => x.Sockets)
            .Must(BeUniqueSocketIds)
            .WithMessage("Duplicate sockets are not allowed.")
            .When(x => x.Sockets.Count > 0);

        RuleForEach(x => x.Sockets).ChildRules(socket =>
        {
            socket.RuleFor(x => x.SocketId)
                .NotEmpty().WithMessage("SocketId is required");
        });
    }

    private static bool BeUniqueSocketIds(List<CpuCoolerSocketDto> sockets) =>
        sockets.GroupBy(x => x.SocketId).All(g => g.Count() == 1);
}
