using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class CreateCpuSeriesCommandValidator: AbstractValidator<CreateCpuSeriesCommand>
{
    public CreateCpuSeriesCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId cannot be empty.");
        RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId cannot be empty.");
    }
}