using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class BulkCreateGpusCommandValidator: AbstractValidator<BulkCreateGpusCommand>
{
    public BulkCreateGpusCommandValidator()
    {
        RuleFor(x => x.Gpus)
            .NotEmpty()
            .WithMessage("Gpus list cannot be empty.");
        
        RuleForEach(x => x.Gpus).ChildRules(gpu =>
        {
            gpu.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(200)
                .WithMessage("Name cannot exceed 200 characters.");
            
            gpu.RuleFor(x => x.ManufacturerId)
                .NotEmpty()
                .WithMessage("ManufacturerId is required.");
            
            gpu.RuleFor(x => x.GpuSeriesId)
                .NotEmpty()
                .WithMessage("GpuSeriesId is required.");
        });
    }
}