using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkCreatePsus;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class BulkCreatePsusCommandValidator : AbstractValidator<BulkCreatePsusCommand>
{
    public BulkCreatePsusCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Psus).NotEmpty();
        RuleFor(x => x.Psus.Select(p => p.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Psus is { Count: > 0 });
        RuleForEach(x => x.Psus).SetValidator(new BulkCreatePsuItemValidator());
    }
}

file sealed class BulkCreatePsuItemValidator : AbstractValidator<CreatePsuItem>
{
    public BulkCreatePsuItemValidator()
    {
        Include(new PsuFieldsValidator<CreatePsuItem>());
        Include(new PsuCableRulesValidator<CreatePsuItem>());
    }
}
