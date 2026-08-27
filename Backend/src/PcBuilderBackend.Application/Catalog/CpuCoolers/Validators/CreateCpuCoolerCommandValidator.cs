using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.CreateCpuCooler;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class CreateCpuCoolerCommandValidator : AbstractValidator<CreateCpuCoolerCommand>
{
    public CreateCpuCoolerCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required")
            .MustBeActiveManufacturer(db);

        RuleFor(x => x.MaxTdp)
            .GreaterThan(0).WithMessage("MaxTdp must be greater than 0");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type is invalid");

        When(x => x.Type == CpuCoolerType.Air, () =>
        {
            RuleFor(x => x.CoolerHeightMm)
                .NotNull().WithMessage("CoolerHeightMm is required for air coolers")
                .GreaterThan(0).WithMessage("CoolerHeightMm must be greater than 0");

            RuleFor(x => x.MaxRamHeightMm)
                .NotNull().WithMessage("MaxRamHeightMm is required for air coolers")
                .GreaterThan(0).WithMessage("MaxRamHeightMm must be greater than 0");

            RuleFor(x => x.RadiatorLength)
                .Null().WithMessage("RadiatorLength must be empty for air coolers");
        });

        When(x => x.Type == CpuCoolerType.Water, () =>
        {
            RuleFor(x => x.RadiatorLength)
                .NotNull().WithMessage("RadiatorLength is required for liquid coolers")
                .IsInEnum().WithMessage("RadiatorLength is invalid");

            RuleFor(x => x.CoolerHeightMm)
                .Null().WithMessage("CoolerHeightMm must be empty for liquid coolers");

            RuleFor(x => x.MaxRamHeightMm)
                .Null().WithMessage("MaxRamHeightMm must be empty for liquid coolers");
        });

        RuleFor(x => x.Sockets)
            .NotEmpty().WithMessage("At least one socket is required")
            .Must(BeUniqueSocketIds)
            .WithMessage("Duplicate sockets are not allowed.");

        RuleFor(x => x.Sockets.Select(s => s.SocketId))
            .MustAllBeActiveSockets(db)
            .When(x => x.Sockets is { Count: > 0 });

        RuleForEach(x => x.Sockets).ChildRules(socket =>
        {
            socket.RuleFor(x => x.SocketId)
                .NotEmpty().WithMessage("SocketId is required");
        });
    }

    private static bool BeUniqueSocketIds(List<CpuCoolerSocketDto> sockets) =>
        sockets.GroupBy(x => x.SocketId).All(g => g.Count() == 1);
}
