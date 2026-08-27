using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkCreateStorageDrives;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class BulkCreateStorageDrivesCommandValidator : AbstractValidator<BulkCreateStorageDrivesCommand>
{
    public BulkCreateStorageDrivesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Drives).NotEmpty();
        RuleFor(x => x.Drives.Select(d => d.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Drives is { Count: > 0 });
        RuleForEach(x => x.Drives).ChildRules(drive =>
        {
            drive.RuleFor(d => d.Name).NotEmpty().MaximumLength(200);
            drive.RuleFor(d => d.ManufacturerId).NotEmpty();
            drive.RuleFor(d => d.Media).IsInEnum();
            drive.RuleFor(d => d.Interface).IsInEnum();
            drive.RuleFor(d => d.FormFactor).IsInEnum();
            drive.RuleFor(d => d.CapacityGb).GreaterThan(0);

            drive.When(d => d.FormFactor is StorageFormFactor.Sata25 or StorageFormFactor.Sata35, () =>
            {
                drive.RuleFor(d => d.Media)
                    .Equal(StorageMedia.Hdd)
                    .WithMessage("Storage media must be HDD for the selected form factor.");
                drive.RuleFor(d => d.Rpm)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("RPM is required for HDD.");
            });

            drive.When(d => d.FormFactor is StorageFormFactor.M22230 or StorageFormFactor.M22242
                or StorageFormFactor.M22260 or StorageFormFactor.M22280 or StorageFormFactor.M222110, () =>
            {
                drive.RuleFor(d => d.Media)
                    .Equal(StorageMedia.Ssd)
                    .WithMessage("Storage media must be SSD for the selected form factor.");
                drive.RuleFor(d => d.PcieGeneration)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("PCIe generation is required for SSD.");
            });
        });
    }
}
