using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.DeleteCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class DeleteCpuSeriesCommandValidator: AbstractValidator<DeleteCpuSeriesCommand>
{
    public DeleteCpuSeriesCommandValidator()
    {
        RuleFor(x => x.CpuSeriesId).NotEmpty().WithMessage("Id cannot be empty.");
    }
}