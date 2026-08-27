using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkUpdateChassisFans;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class BulkUpdateChassisFansCommandValidator : AbstractValidator<BulkUpdateChassisFansCommand>
{
    public BulkUpdateChassisFansCommandValidator()
    {
        RuleFor(x => x.Fans).NotEmpty();
        RuleForEach(x => x.Fans).SetValidator(new UpdateChassisFanCommandValidator());
    }
}
