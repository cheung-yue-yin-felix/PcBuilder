using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class CreateGpuSeriesCommandValidator: AbstractValidator<CreateGpuSeriesCommand>
{
    public CreateGpuSeriesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.")
            .MustBeActiveManufacturer(db);
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters.");
    }
}