using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardUsbPorts;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class BulkUpdateMotherboardUsbPortsCommandValidator
    : AbstractValidator<BulkUpdateMotherboardUsbPortsCommand>
{
    public BulkUpdateMotherboardUsbPortsCommandValidator()
    {
        RuleFor(x => x.MotherboardId).NotEmpty();

        RuleFor(x => x.UsbPorts)
            .Must(ports => ports
                .Select(p => (p.UsbType, p.UsbVersion))
                .Distinct()
                .Count() == ports.Count)
            .WithMessage("Duplicate USB ports (same type and version) are not allowed.")
            .When(x => x.UsbPorts.Count > 0);

        RuleForEach(x => x.UsbPorts).ChildRules(port =>
        {
            port.RuleFor(x => x.UsbVersion).IsInEnum();
            port.RuleFor(x => x.UsbType).IsInEnum();
            port.RuleFor(x => x.PortCount).GreaterThan(0);
        });
    }
}
