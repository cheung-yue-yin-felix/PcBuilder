using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkCreateWiredNetworkAdapters;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class BulkCreateWiredNetworkAdaptersCommandValidator : AbstractValidator<BulkCreateWiredNetworkAdaptersCommand>
{
    public BulkCreateWiredNetworkAdaptersCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleFor(x => x.Adapters.Select(a => a.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Adapters is { Count: > 0 });
        RuleForEach(x => x.Adapters).ChildRules(adapter =>
        {
            adapter.RuleFor(a => a.Name).NotEmpty().MaximumLength(200);
            adapter.RuleFor(a => a.ManufacturerId).NotEmpty();
            adapter.RuleFor(a => a.HostInterface).IsInEnum();
            adapter.RuleFor(a => a.MaxSpeedMbps).GreaterThan(0);

            adapter.When(a => a.HostInterface == WiredHostInterface.Usb, () =>
            {
                adapter.RuleFor(a => a.UsbVersion)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("USB version is required for USB wired adapters.");
                adapter.RuleFor(a => a.UsbType)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("USB type is required for USB wired adapters.");
                adapter.RuleFor(a => a.PcieSlotType)
                    .Null()
                    .WithMessage("PCIe slot type is not valid for USB wired adapters.");
            });

            adapter.When(a => a.HostInterface == WiredHostInterface.Pcie, () =>
            {
                adapter.RuleFor(a => a.PcieSlotType)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("PCIe slot type is required for PCIe wired adapters.");
                adapter.RuleFor(a => a.UsbVersion)
                    .Null()
                    .WithMessage("USB fields are not valid for PCIe wired adapters.");
                adapter.RuleFor(a => a.UsbType)
                    .Null()
                    .WithMessage("USB fields are not valid for PCIe wired adapters.");
            });
        });
    }
}
