using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.CreatePsu;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class CreatePsuCommandValidator : AbstractValidator<CreatePsuCommand>
{
    public CreatePsuCommandValidator(IActiveEntityLookup db)
    {
        Include(new PsuFieldsValidator<CreatePsuCommand>());
        Include(new PsuCableRulesValidator<CreatePsuCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
