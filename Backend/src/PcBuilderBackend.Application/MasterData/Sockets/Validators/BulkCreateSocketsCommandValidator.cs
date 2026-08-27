using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class BulkCreateSocketsCommandValidator: AbstractValidator<BulkCreateSocketsCommand>
{
    public BulkCreateSocketsCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Sockets).NotEmpty().WithMessage("Sockets list cannot be empty.");
        RuleFor(x => x.Sockets.Select(s => s.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Sockets is { Count: > 0 });

        RuleForEach(x => x.Sockets).ChildRules(sockets =>
        {
            sockets.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required.");
            sockets.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
        });
    }
}