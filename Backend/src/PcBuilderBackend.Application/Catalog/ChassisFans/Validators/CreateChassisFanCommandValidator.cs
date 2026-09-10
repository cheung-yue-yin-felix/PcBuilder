using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.CreateChassisFan;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class CreateChassisFanCommandValidator : AbstractValidator<CreateChassisFanCommand>
{
    public CreateChassisFanCommandValidator(IActiveEntityLookup db)
    {
        Include(new ChassisFanFieldsValidator<CreateChassisFanCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
