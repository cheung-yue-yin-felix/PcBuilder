using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkUpdateGpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class BulkUpdateGpusCommandValidator: AbstractValidator<BulkUpdateGpusCommand>
{
    public BulkUpdateGpusCommandValidator()
    {
        RuleFor(x => x.Gpus).NotEmpty().WithMessage("At least one GPU is required");
        RuleForEach(x => x.Gpus).ChildRules(gpu =>
        {
            gpu.RuleFor(x => x.Id).NotEmpty().WithMessage("GPU Id is required");
            gpu.RuleFor(x => x.Name)
                .NotEmpty().WithMessage("GPU Name is required")
                .MaximumLength(200).WithMessage("GPU Name cannot exceed 200 characters");
            gpu.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required");
            gpu.RuleFor(x => x.GpuSeriesId).NotEmpty().WithMessage("GpuSeriesId is required");
        });
    }
}