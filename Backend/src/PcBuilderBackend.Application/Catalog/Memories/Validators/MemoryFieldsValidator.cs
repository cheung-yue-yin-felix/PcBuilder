using FluentValidation;
using PcBuilderBackend.Application.Catalog.Memories;

namespace PcBuilderBackend.Application.Catalog.Memories.Validators;

public sealed class MemoryFieldsValidator<T> : AbstractValidator<T>
    where T : IMemoryFields
{
    public MemoryFieldsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required")
            .MaximumLength(200);

        RuleFor(x => x.DdrGeneration)
            .IsInEnum().WithMessage("DdrGeneration is invalid");

        RuleFor(x => x.RamFormFactor)
            .IsInEnum().WithMessage("RamFormFactor is invalid");

        RuleFor(x => x.RamRank)
            .IsInEnum().WithMessage("RamRank is invalid");

        RuleFor(x => x.MemorySizePerStickGb)
            .GreaterThan(0).WithMessage("MemorySizePerStickGb must be greater than 0");

        RuleFor(x => x.TotalMemorySizeGb)
            .GreaterThan(0).WithMessage("TotalMemorySizeGb must be greater than 0");

        RuleFor(x => x)
            .Must(x => x.TotalMemorySizeGb >= x.MemorySizePerStickGb)
            .WithMessage("TotalMemorySizeGb must be greater than or equal to MemorySizePerStickGb");

        RuleFor(x => x.ModulesCount)
            .GreaterThan(0).WithMessage("ModulesCount must be greater than 0");

        RuleFor(x => x.MaxMemorySpeedMts)
            .GreaterThan(0).WithMessage("MaxMemorySpeedMts must be greater than 0");

        RuleFor(x => x.HeightMm)
            .GreaterThan(0).WithMessage("HeightMm must be greater than 0");
    }
}
