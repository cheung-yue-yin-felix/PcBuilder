using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkCreateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class BulkCreateGpuSeriesCommandValidator: AbstractValidator<BulkCreateGpuSeriesCommand>
{
    public BulkCreateGpuSeriesCommandValidator()
    {
        RuleFor(x =>x.GpuSeries).NotEmpty().WithMessage("GpuSeries list cannot be empty.");
        
        RuleForEach(x => x.GpuSeries).ChildRules(gpuSeries =>
        {
            gpuSeries.RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("GpuSeries name cannot be empty.")
                .MaximumLength(200)
                .WithMessage("GpuSeries name cannot exceed 200 characters.");
            
            gpuSeries.RuleFor(g => g.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID cannot be empty.");
        });
    }
}