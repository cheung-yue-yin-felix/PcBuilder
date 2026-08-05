using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class BulkCreateCpuSeriesCommandValidator: AbstractValidator<BulkCreateCpuSeriesCommand>
{
    public BulkCreateCpuSeriesCommandValidator()
    {
        RuleFor(x => x.CpuSeries).NotEmpty().WithMessage("CpuSeries cannot be empty.");

        RuleForEach(x => x.CpuSeries).ChildRules(series =>
        {
            series.RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            series.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId cannot be empty.");
            series.RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId cannot be empty.");
        });
    }
}