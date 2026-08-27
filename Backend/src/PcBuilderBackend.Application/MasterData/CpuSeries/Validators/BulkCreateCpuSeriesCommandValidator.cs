using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class BulkCreateCpuSeriesCommandValidator: AbstractValidator<BulkCreateCpuSeriesCommand>
{
    public BulkCreateCpuSeriesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.CpuSeries).NotEmpty().WithMessage("CpuSeries cannot be empty.");
        RuleFor(x => x.CpuSeries.Select(s => s.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.CpuSeries is { Count: > 0 });
        RuleFor(x => x.CpuSeries.Select(s => s.SocketId))
            .MustAllBeActiveSockets(db)
            .When(x => x.CpuSeries is { Count: > 0 });

        RuleForEach(x => x.CpuSeries).ChildRules(series =>
        {
            series.RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            series.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId cannot be empty.");
            series.RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId cannot be empty.");
        });
    }
}