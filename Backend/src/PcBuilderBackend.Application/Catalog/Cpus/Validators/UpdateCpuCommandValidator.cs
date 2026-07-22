using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class UpdateCpuCommandValidator: AbstractValidator<UpdateCpuCommand>
{
    public UpdateCpuCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required");
        
        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required");
        
        RuleFor(x => x.SeriesId)
            .NotEmpty().WithMessage("SeriesId is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.DdrGeneration)
            .NotEmpty().WithMessage("DdrGeneration is required");
        
        RuleFor(x => x.MaxMemoryGb)
            .GreaterThan(0).WithMessage("MaxMemoryGb must be greater than 0");
        
        RuleFor(x => x.ThermalDesignPower)
            .GreaterThan(0).WithMessage("ThermalDesignPower is greater than 0");
    }
}