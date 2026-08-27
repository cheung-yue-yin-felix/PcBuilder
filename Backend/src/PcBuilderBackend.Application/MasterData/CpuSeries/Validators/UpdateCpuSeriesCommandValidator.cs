using FluentValidation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.UpdateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class UpdateCpuSeriesCommandValidator: AbstractValidator<UpdateCpuSeriesCommand>
{
    public UpdateCpuSeriesCommandValidator()
    {
        RuleFor(x => x.CpuSeriesId).NotEmpty().WithMessage("CpuSeriesId cannot be empty.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .Length(200).WithMessage("Name cannot be more than 200 characters.");
        
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId cannot be empty.");
        RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId cannot be empty.");
    }
}