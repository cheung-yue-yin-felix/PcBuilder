using FluentValidation;
using PcBuilderBackend.Application.Auth.Commands.RefreshToken;

namespace PcBuilderBackend.Application.Auth.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
