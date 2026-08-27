using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class CreateGpuCommandValidator: AbstractValidator<CreateGpuCommand>
{
    public CreateGpuCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters.");
        
        RuleFor(x => x.ManufacturerId)
            .NotEmpty()
            .WithMessage("ManufacturerId is required.")
            .MustBeActiveManufacturer(db);
        
        RuleFor(x => x.GpuSeriesId)
            .NotEmpty()
            .WithMessage("GpuSeriesId is required.")
            .MustBeActiveGpuSeries(db);
    }
}