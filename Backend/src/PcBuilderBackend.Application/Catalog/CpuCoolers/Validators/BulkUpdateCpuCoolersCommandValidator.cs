using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkUpdateCpuCoolers;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class BulkUpdateCpuCoolersCommandValidator : AbstractValidator<BulkUpdateCpuCoolersCommand>
{
    public BulkUpdateCpuCoolersCommandValidator()
    {
        RuleFor(x => x.CpuCoolers)
            .NotEmpty().WithMessage("At least one CPU cooler is required");

        RuleForEach(x => x.CpuCoolers).ChildRules(cooler =>
        {
            cooler.RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            cooler.RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200);

            cooler.RuleFor(x => x.ManufacturerId)
                .NotEmpty().WithMessage("ManufacturerId is required");

            cooler.RuleFor(x => x.MaxTdp)
                .GreaterThan(0).WithMessage("MaxTdp must be greater than 0");

            cooler.RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Type is invalid");

            cooler.When(x => x.Type == CpuCoolerType.Air, () =>
            {
                cooler.RuleFor(x => x.CoolerHeightMm)
                    .NotNull().WithMessage("CoolerHeightMm is required for air coolers")
                    .GreaterThan(0).WithMessage("CoolerHeightMm must be greater than 0");

                cooler.RuleFor(x => x.MaxRamHeightMm)
                    .NotNull().WithMessage("MaxRamHeightMm is required for air coolers")
                    .GreaterThan(0).WithMessage("MaxRamHeightMm must be greater than 0");

                cooler.RuleFor(x => x.RadiatorLength)
                    .Null().WithMessage("RadiatorLength must be empty for air coolers");
            });

            cooler.When(x => x.Type == CpuCoolerType.Water, () =>
            {
                cooler.RuleFor(x => x.RadiatorLength)
                    .NotNull().WithMessage("RadiatorLength is required for liquid coolers")
                    .IsInEnum().WithMessage("RadiatorLength is invalid");

                cooler.RuleFor(x => x.CoolerHeightMm)
                    .Null().WithMessage("CoolerHeightMm must be empty for liquid coolers");

                cooler.RuleFor(x => x.MaxRamHeightMm)
                    .Null().WithMessage("MaxRamHeightMm must be empty for liquid coolers");
            });
        });
    }
}
