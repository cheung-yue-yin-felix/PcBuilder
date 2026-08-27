using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class BulkCreateGpusCommandValidator: AbstractValidator<BulkCreateGpusCommand>
{
    public BulkCreateGpusCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Gpus)
            .NotEmpty()
            .WithMessage("Gpus list cannot be empty.");

        RuleFor(x => x.Gpus.Select(g => g.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Gpus is { Count: > 0 });
        RuleFor(x => x.Gpus.Select(g => g.GpuSeriesId))
            .MustAllBeActiveGpuSeries(db)
            .When(x => x.Gpus is { Count: > 0 });
        
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