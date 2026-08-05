using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class BulkCreateSocketsCommandValidator: AbstractValidator<BulkCreateSocketsCommand>
{
    public BulkCreateSocketsCommandValidator()
    {
        RuleForEach(x => x.Sockets).ChildRules(sockets =>
        {
            sockets.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required.");
            sockets.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
        });
    }
}