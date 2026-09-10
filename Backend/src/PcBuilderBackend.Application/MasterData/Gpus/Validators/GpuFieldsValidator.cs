using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public sealed class GpuFieldsValidator<T> : AbstractValidator<T>
    where T : IGpuFields
{
    public GpuFieldsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty()
            .WithMessage("ManufacturerId is required.");

        RuleFor(x => x.GpuSeriesId)
            .NotEmpty()
            .WithMessage("GpuSeriesId is required.");
    }
}
