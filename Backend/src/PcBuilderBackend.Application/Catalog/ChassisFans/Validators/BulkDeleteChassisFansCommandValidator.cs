using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkDeleteChassisFans;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class BulkDeleteChassisFansCommandValidator : AbstractValidator<BulkDeleteChassisFansCommand>
{
    public BulkDeleteChassisFansCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
