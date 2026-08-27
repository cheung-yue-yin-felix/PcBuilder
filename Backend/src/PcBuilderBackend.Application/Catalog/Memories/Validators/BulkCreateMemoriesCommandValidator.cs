using FluentValidation;
using PcBuilderBackend.Application.Catalog.Memories.Commands.BulkCreateMemories;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Memories.Validators;

public class BulkCreateMemoriesCommandValidator: AbstractValidator<BulkCreateMemoriesCommand>
{
    public BulkCreateMemoriesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Memories).NotEmpty().WithMessage("At least one RAM module is required");
        RuleFor(x => x.Memories.Select(m => m.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Memories is { Count: > 0 });

        RuleForEach(x => x.Memories).ChildRules(memory =>
        {
            memory.RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200);

            memory.RuleFor(x => x.ManufacturerId)
                .NotEmpty().WithMessage("ManufacturerId is required");

            memory.RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color is required")
                .MaximumLength(200);

            memory.RuleFor(x => x.DdrGeneration)
                .IsInEnum().WithMessage("DdrGeneration is invalid");

            memory.RuleFor(x => x.RamFormFactor)
                .IsInEnum().WithMessage("RamFormFactor is invalid");

            memory.RuleFor(x => x.RamRank)
                .IsInEnum().WithMessage("RamRank is invalid");

            memory.RuleFor(x => x.MemorySizePerStickGb)
                .GreaterThan(0).WithMessage("MemorySizePerStickGb must be greater than 0");

            memory.RuleFor(x => x.TotalMemorySizeGb)
                .GreaterThan(0).WithMessage("TotalMemorySizeGb must be greater than 0");

            memory.RuleFor(x => x)
                .Must(x => x.TotalMemorySizeGb >= x.MemorySizePerStickGb)
                .WithMessage("TotalMemorySizeGb must be greater than or equal to MemorySizePerStickGb");

            memory.RuleFor(x => x.ModulesCount)
                .GreaterThan(0).WithMessage("ModulesCount must be greater than 0");

            memory.RuleFor(x => x.MaxMemorySpeedMts)
                .GreaterThan(0).WithMessage("MaxMemorySpeedMts must be greater than 0");

            memory.RuleFor(x => x.HeightMm)
                .GreaterThan(0).WithMessage("HeightMm must be greater than 0");
        });
    }    
}