using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.UpdatePsu;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class UpdatePsuCommandValidator : AbstractValidator<UpdatePsuCommand>
{
    public UpdatePsuCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new PsuFieldsValidator<UpdatePsuCommand>());
    }
}
