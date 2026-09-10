using FluentValidation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class UpdateGpuSeriesCommandValidator : AbstractValidator<UpdateGpuSeriesCommand>
{
    public UpdateGpuSeriesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required.");
        Include(new GpuSeriesFieldsValidator<UpdateGpuSeriesCommand>());
    }
}
