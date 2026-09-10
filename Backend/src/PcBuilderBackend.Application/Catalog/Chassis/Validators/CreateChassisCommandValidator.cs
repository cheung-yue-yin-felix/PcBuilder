using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.CreateChassis;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class CreateChassisCommandValidator : AbstractValidator<CreateChassisCommand>
{
    public CreateChassisCommandValidator(IActiveEntityLookup db)
    {
        Include(new ChassisFieldsValidator<CreateChassisCommand>());
        Include(new ChassisCollectionRulesValidator<CreateChassisCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
