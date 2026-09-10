using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkCreateChassis;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkCreateChassisCommandValidator : AbstractValidator<BulkCreateChassisCommand>
{
    public BulkCreateChassisCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Items).NotEmpty();
        RuleFor(x => x.Items.Select(c => c.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Items is { Count: > 0 });
        RuleForEach(x => x.Items).SetValidator(new BulkCreateChassisItemValidator());
    }
}

file sealed class BulkCreateChassisItemValidator : AbstractValidator<CreateChassisItem>
{
    public BulkCreateChassisItemValidator()
    {
        Include(new ChassisFieldsValidator<CreateChassisItem>());
        Include(new ChassisCollectionRulesValidator<CreateChassisItem>());
    }
}
