using System.Data;
using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class BulkDeleteCpuSeriesCommandValidator: AbstractValidator<BulkDeleteCpuSeriesCommand>
{
    public BulkDeleteCpuSeriesCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids cannot be empty.");
    }
}