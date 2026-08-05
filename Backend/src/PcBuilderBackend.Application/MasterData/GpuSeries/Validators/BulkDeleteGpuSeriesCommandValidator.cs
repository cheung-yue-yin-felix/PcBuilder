using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkDeleteGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class BulkDeleteGpuSeriesCommandValidator: AbstractValidator<BulkDeleteGpuSeriesCommand>
{
    public BulkDeleteGpuSeriesCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("GPU Series IDs list cannot be empty.");
    }
}