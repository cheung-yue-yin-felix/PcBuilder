using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.CreateChassisFan;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class CreateChassisFanCommandValidator : AbstractValidator<CreateChassisFanCommand>
{
    public CreateChassisFanCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty().MustBeActiveManufacturer(db);
        RuleFor(x => x.DiameterMm).IsInEnum();
        RuleFor(x => x.FansCountPerPack).GreaterThan(0);
    }
}
