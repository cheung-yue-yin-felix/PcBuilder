using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
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
        RuleForEach(x => x.Cpus).SetValidator(new BulkCreateCpuItemValidator());
    }
}

file sealed class BulkCreateCpuItemValidator : AbstractValidator<CpuDto>
{
    public BulkCreateCpuItemValidator()
    {
        Include(new CpuFieldsValidator<CpuDto>());
        RuleFor(x => x.RamCompats).NotEmpty().WithMessage("At least one RAM compatibility entry is required");
        RuleFor(x => x.SupportChipsets)
            .NotEmpty().WithMessage("At least one supported chipset entry is required")
            .Must(supports => supports.GroupBy(s => s.ChipsetId).All(g => g.Count() == 1))
            .WithMessage("Duplicate chipset support entries are not allowed.");
        RuleForEach(x => x.SupportChipsets).ChildRules(support =>
        {
            support.RuleFor(x => x.ChipsetId).NotEmpty().WithMessage("ChipsetId is required");
        });
    }
}
