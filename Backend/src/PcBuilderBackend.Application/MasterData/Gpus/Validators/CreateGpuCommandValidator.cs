using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class CreateGpuCommandValidator : AbstractValidator<CreateGpuCommand>
{
    public CreateGpuCommandValidator(IActiveEntityLookup db)
    {
        Include(new GpuFieldsValidator<CreateGpuCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
        RuleFor(x => x.GpuSeriesId).MustBeActiveGpuSeries(db);
    }
}
