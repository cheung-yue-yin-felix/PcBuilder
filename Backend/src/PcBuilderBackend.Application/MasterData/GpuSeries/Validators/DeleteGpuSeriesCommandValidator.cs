using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.DeleteGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class DeleteGpuSeriesCommandValidator: AbstractValidator<DeleteGpuSeriesCommand>
{
    public DeleteGpuSeriesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("GpuSeriesId is required.");
    }
}