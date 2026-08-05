using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class BulkCreateCpusCommandValidator : AbstractValidator<BulkCreateCpusCommand>
{
    public BulkCreateCpusCommandValidator()
    {
        RuleFor(x => x.Cpus).NotEmpty().WithMessage("At least one CPU is required");
        RuleForEach(x => x.Cpus).ChildRules(cpu =>
        {
            cpu.RuleFor(x => x.Name)
                .NotEmpty().WithMessage("CPU Name is required")
                .MaximumLength(200).WithMessage("CPU Name cannot exceed 200 characters");
            cpu.RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required");
            cpu.RuleFor(x => x.SocketId).NotEmpty().WithMessage("SocketId is required");
            cpu.RuleFor(x => x.SeriesId).NotEmpty().WithMessage("SeriesId is required");
            cpu.RuleFor(x => x.MaxMemoryGb).GreaterThan(0).WithMessage("MaxMemoryGb must be greater than 0");
            cpu.RuleFor(x => x.ThermalDesignPower).GreaterThan(0).WithMessage("ThermalDesignPower must be greater than 0");
            cpu.RuleFor(x => x.RamCompats).NotEmpty().WithMessage("At least one RAM compatibility entry is required");
        });
    }
}
