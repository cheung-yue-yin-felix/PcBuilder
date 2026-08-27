using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class BulkCreateCpusCommandValidator : AbstractValidator<BulkCreateCpusCommand>
{
    public BulkCreateCpusCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Cpus).NotEmpty().WithMessage("At least one CPU is required");
        RuleFor(x => x.Cpus.Select(c => c.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Cpus is { Count: > 0 });
        RuleFor(x => x.Cpus.Select(c => c.SocketId))
            .MustAllBeActiveSockets(db)
            .When(x => x.Cpus is { Count: > 0 });
        RuleFor(x => x.Cpus.Select(c => c.SeriesId))
            .MustAllBeActiveCpuSeries(db)
            .When(x => x.Cpus is { Count: > 0 });
        RuleFor(x => x.Cpus.SelectMany(c => c.SupportChipsets.Select(s => s.ChipsetId)))
            .MustAllBeActiveChipsets(db)
            .When(x => x.Cpus is { Count: > 0 });
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
            cpu.RuleFor(x => x.PowerConsumptionWatts).GreaterThan(0).WithMessage("PowerConsumptionWatts must be greater than 0");
            cpu.RuleFor(x => x.RamCompats).NotEmpty().WithMessage("At least one RAM compatibility entry is required");
            cpu.RuleFor(x => x.SupportChipsets)
                .NotEmpty().WithMessage("At least one supported chipset entry is required")
                .Must(supports => supports.GroupBy(s => s.ChipsetId).All(g => g.Count() == 1))
                .WithMessage("Duplicate chipset support entries are not allowed.");
            cpu.RuleForEach(x => x.SupportChipsets).ChildRules(support =>
            {
                support.RuleFor(x => x.ChipsetId).NotEmpty().WithMessage("ChipsetId is required");
            });
        });
    }
}
