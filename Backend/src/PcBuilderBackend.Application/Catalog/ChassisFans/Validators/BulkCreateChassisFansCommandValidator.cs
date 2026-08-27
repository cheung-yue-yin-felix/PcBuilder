using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkCreateChassisFans;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class BulkCreateChassisFansCommandValidator : AbstractValidator<BulkCreateChassisFansCommand>
{
    public BulkCreateChassisFansCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Fans).NotEmpty();
        RuleFor(x => x.Fans.Select(f => f.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Fans is { Count: > 0 });
        RuleForEach(x => x.Fans).ChildRules(fan =>
        {
            fan.RuleFor(f => f.Name).NotEmpty().MaximumLength(200);
            fan.RuleFor(f => f.ManufacturerId).NotEmpty();
            fan.RuleFor(f => f.DiameterMm).IsInEnum();
            fan.RuleFor(f => f.FansCountPerPack).GreaterThan(0);
        });
    }
}
