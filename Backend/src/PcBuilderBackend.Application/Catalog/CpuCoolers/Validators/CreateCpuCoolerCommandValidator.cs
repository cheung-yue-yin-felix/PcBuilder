using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.CreateCpuCooler;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class CreateCpuCoolerCommandValidator : AbstractValidator<CreateCpuCoolerCommand>
{
    public CreateCpuCoolerCommandValidator(IActiveEntityLookup db)
    {
        Include(new CpuCoolerFieldsValidator<CreateCpuCoolerCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);

        RuleFor(x => x.Sockets)
            .NotEmpty().WithMessage("At least one socket is required")
            .Must(BeUniqueSocketIds)
            .WithMessage("Duplicate sockets are not allowed.");

        RuleFor(x => x.Sockets.Select(s => s.SocketId))
            .MustAllBeActiveSockets(db)
            .When(x => x.Sockets is { Count: > 0 });

        RuleForEach(x => x.Sockets).ChildRules(socket =>
        {
            socket.RuleFor(x => x.SocketId)
                .NotEmpty().WithMessage("SocketId is required");
        });
    }

    private static bool BeUniqueSocketIds(List<CpuCoolerSocketDto> sockets) =>
        sockets.GroupBy(x => x.SocketId).All(g => g.Count() == 1);
}
