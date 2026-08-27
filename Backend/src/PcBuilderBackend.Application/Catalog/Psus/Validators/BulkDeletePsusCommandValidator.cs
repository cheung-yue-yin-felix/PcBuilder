using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkDeletePsus;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class BulkDeletePsusCommandValidator : AbstractValidator<BulkDeletePsusCommand>
{
    public BulkDeletePsusCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
