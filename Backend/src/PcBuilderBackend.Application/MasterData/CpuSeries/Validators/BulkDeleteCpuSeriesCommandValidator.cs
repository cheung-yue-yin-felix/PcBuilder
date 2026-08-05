using System.Data;
using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class BulkDeleteCpuSeriesCommandValidator: AbstractValidator<BulkDeleteCpuSeriesCommand>
{
    public BulkDeleteCpuSeriesCommandValidator()
    {
        RuleFor(x => x.CpuSeriesIds).NotEmpty().WithMessage("CpuSeriesIds cannot be empty.");
    }
}