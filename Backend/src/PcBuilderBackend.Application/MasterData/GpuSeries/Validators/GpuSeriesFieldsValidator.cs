using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public sealed class GpuSeriesFieldsValidator<T> : AbstractValidator<T>
    where T : IGpuSeriesFields
{
    public GpuSeriesFieldsValidator()
    {
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}
