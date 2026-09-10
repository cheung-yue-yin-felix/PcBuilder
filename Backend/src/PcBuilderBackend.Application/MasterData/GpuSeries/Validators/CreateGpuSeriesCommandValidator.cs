using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Validators;

public class CreateGpuSeriesCommandValidator : AbstractValidator<CreateGpuSeriesCommand>
{
    public CreateGpuSeriesCommandValidator(IActiveEntityLookup db)
    {
        Include(new GpuSeriesFieldsValidator<CreateGpuSeriesCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
