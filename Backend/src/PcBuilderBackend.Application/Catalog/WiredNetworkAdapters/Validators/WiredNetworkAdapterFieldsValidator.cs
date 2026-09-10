using FluentValidation;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public sealed class WiredNetworkAdapterFieldsValidator<T> : AbstractValidator<T>
    where T : IWiredNetworkAdapterFields
{
    public WiredNetworkAdapterFieldsValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.HostInterface).IsInEnum();
        RuleFor(x => x.MaxSpeedMbps).GreaterThan(0);

        When(x => x.HostInterface == WiredHostInterface.Usb, () =>
        {
            RuleFor(x => x.UsbVersion)
                .NotNull()
                .IsInEnum()
                .WithMessage("USB version is required for USB wired adapters.");
            RuleFor(x => x.UsbType)
                .NotNull()
                .IsInEnum()
                .WithMessage("USB type is required for USB wired adapters.");
            RuleFor(x => x.PcieSlotType)
                .Null()
                .WithMessage("PCIe slot type is not valid for USB wired adapters.");
        });

        When(x => x.HostInterface == WiredHostInterface.Pcie, () =>
        {
            RuleFor(x => x.PcieSlotType)
                .NotNull()
                .IsInEnum()
                .WithMessage("PCIe slot type is required for PCIe wired adapters.");
            RuleFor(x => x.UsbVersion)
                .Null()
                .WithMessage("USB fields are not valid for PCIe wired adapters.");
            RuleFor(x => x.UsbType)
                .Null()
                .WithMessage("USB fields are not valid for PCIe wired adapters.");
        });
    }
}
