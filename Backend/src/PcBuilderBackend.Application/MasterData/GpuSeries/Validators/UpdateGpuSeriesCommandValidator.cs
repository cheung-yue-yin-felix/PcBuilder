using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class UpdateGpuSeriesCommandValidator: AbstractValidator<UpdateGpuSeriesCommand>
{
    public UpdateGpuSeriesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required.");
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}