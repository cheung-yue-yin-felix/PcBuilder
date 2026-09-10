using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkCreateCpuCoolers;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class BulkCreateCpuCoolersCommandValidator : AbstractValidator<BulkCreateCpuCoolersCommand>
{
    public BulkCreateCpuCoolersCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.CpuCoolers)
            .NotEmpty().WithMessage("At least one CPU cooler is required");

        RuleFor(x => x.CpuCoolers.Select(c => c.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.CpuCoolers is { Count: > 0 });
        RuleFor(x => x.CpuCoolers.SelectMany(c => c.Sockets.Select(s => s.SocketId)))
            .MustAllBeActiveSockets(db)
            .When(x => x.CpuCoolers is { Count: > 0 });

        RuleForEach(x => x.CpuCoolers).SetValidator(new BulkCreateCpuCoolerItemValidator());
    }
}

file sealed class BulkCreateCpuCoolerItemValidator : AbstractValidator<CpuCoolerDto>
{
    public BulkCreateCpuCoolerItemValidator()
    {
        Include(new CpuCoolerFieldsValidator<CpuCoolerDto>());
        RuleFor(x => x.Sockets)
            .NotEmpty().WithMessage("At least one socket is required")
            .Must(sockets => sockets.GroupBy(s => s.SocketId).All(g => g.Count() == 1))
            .WithMessage("Duplicate sockets are not allowed.");
        RuleForEach(x => x.Sockets).ChildRules(socket =>
        {
            socket.RuleFor(x => x.SocketId)
                .NotEmpty().WithMessage("SocketId is required");
        });
    }
}
