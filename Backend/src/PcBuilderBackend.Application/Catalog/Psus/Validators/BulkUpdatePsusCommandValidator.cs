using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsus;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class BulkUpdatePsusCommandValidator : AbstractValidator<BulkUpdatePsusCommand>
{
    public BulkUpdatePsusCommandValidator()
    {
        RuleFor(x => x.Psus).NotEmpty();
        RuleForEach(x => x.Psus).SetValidator(new UpdatePsuCommandValidator());
    }
}
