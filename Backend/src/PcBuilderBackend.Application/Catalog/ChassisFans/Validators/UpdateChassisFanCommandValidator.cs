using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.UpdateChassisFan;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class UpdateChassisFanCommandValidator : AbstractValidator<UpdateChassisFanCommand>
{
    public UpdateChassisFanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.DiameterMm).IsInEnum();
        RuleFor(x => x.FansCountPerPack).GreaterThan(0);
    }
}
