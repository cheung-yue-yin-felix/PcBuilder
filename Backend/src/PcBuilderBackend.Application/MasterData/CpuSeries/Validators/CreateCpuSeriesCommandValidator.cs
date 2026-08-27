using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Validators;

public class CreateCpuSeriesCommandValidator: AbstractValidator<CreateCpuSeriesCommand>
{
    public CreateCpuSeriesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId cannot be empty.")
            .MustBeActiveManufacturer(db);
        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId cannot be empty.")
            .MustBeActiveSocket(db);
    }
}