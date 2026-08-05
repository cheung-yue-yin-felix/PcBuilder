using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkUpdateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class BulkUpdateGpuSeriesCommandValidator: AbstractValidator<BulkUpdateGpuSeriesCommand>
{
    public BulkUpdateGpuSeriesCommandValidator()
    {
        RuleFor(x => x.GpuSeries).NotEmpty().WithMessage("GPU Series list cannot be empty.");
        
        RuleForEach(x => x.GpuSeries).ChildRules(gpuSeries =>
        {
            gpuSeries.RuleFor(g => g.Id).NotEmpty().WithMessage("GPU Series ID cannot be empty.");
            gpuSeries.RuleFor(g => g.Name).NotEmpty().WithMessage("GPU Series name cannot be empty.").MaximumLength(200).WithMessage("GPU Series name cannot exceed 200 characters.");
            gpuSeries.RuleFor(g => g.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID cannot be empty.");
        });
    }
}