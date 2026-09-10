using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public sealed class CpuSeriesFieldsValidator<T> : AbstractValidator<T>
    where T : ICpuSeriesFields
{
    public CpuSeriesFieldsValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId cannot be empty.");
        RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId cannot be empty.");
    }
}
