using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class CreateCpuSeriesCommandValidator : AbstractValidator<CreateCpuSeriesCommand>
{
    public CreateCpuSeriesCommandValidator(IActiveEntityLookup db)
    {
        Include(new CpuSeriesFieldsValidator<CreateCpuSeriesCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
        RuleFor(x => x.SocketId).MustBeActiveSocket(db);
    }
}
