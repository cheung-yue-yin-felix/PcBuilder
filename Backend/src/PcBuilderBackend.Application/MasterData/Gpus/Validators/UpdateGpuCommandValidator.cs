using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class UpdateGpuCommandValidator: AbstractValidator<UpdateGpuCommand>
{
    
    public UpdateGpuCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        
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