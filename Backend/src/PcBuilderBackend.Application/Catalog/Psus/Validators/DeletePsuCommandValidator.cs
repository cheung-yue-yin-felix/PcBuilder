using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.DeletePsu;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class DeletePsuCommandValidator : AbstractValidator<DeletePsuCommand>
{
    public DeletePsuCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
