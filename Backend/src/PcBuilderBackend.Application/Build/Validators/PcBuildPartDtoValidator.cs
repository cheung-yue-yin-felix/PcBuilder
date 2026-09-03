using FluentValidation;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Build.Validators;

internal static class PcBuildPartRules
{
    public static void Parts<T>(
        IRuleBuilderInitialCollection<T, PcBuildPartDto> rule,
        PcBuildPartType expectedType) =>
        rule.ChildRules(part =>
        {
            part.RuleFor(x => x.PartId).NotEmpty();
            part.RuleFor(x => x.Quantity).GreaterThan(0);
            part.RuleFor(x => x.Type).Equal(expectedType);
        });
}
