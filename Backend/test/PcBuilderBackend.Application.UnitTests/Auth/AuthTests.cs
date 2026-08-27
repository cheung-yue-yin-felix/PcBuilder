using FluentAssertions;
using NSubstitute;
using PcBuilderBackend.Application.Auth.Commands.Login;
using PcBuilderBackend.Application.Auth.Commands.Logout;
using PcBuilderBackend.Application.Auth.Commands.RefreshToken;
using PcBuilderBackend.Application.Auth.Commands.Register;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Auth.Queries;
using PcBuilderBackend.Application.Auth.Validators;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.UnitTests.Auth;

public class AuthTests
{
    [Fact]
    public void Register_validator_requires_email_password_and_names()
    {
        var validator = new RegisterCommandValidator();

        validator.Validate(new RegisterCommand("", "short", "", "")).IsValid.Should().BeFalse();
        validator.Validate(new RegisterCommand("user@localhost", "ChangeMe!12", "Ada", "Lovelace"))
            .IsValid.Should().BeTrue();
    }

    [Fact]
    public void Login_and_refresh_validators_reject_empty_fields()
    {
        new LoginCommandValidator().Validate(new LoginCommand("", "")).IsValid.Should().BeFalse();
        new LoginCommandValidator().Validate(new LoginCommand("a@b.c", "secret")).IsValid.Should().BeTrue();
        new RefreshTokenCommandValidator().Validate(new RefreshTokenCommand("")).IsValid.Should().BeFalse();
        new RefreshTokenCommandValidator().Validate(new RefreshTokenCommand("token")).IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Register_handler_delegates_to_identity_service()
    {
        var identity = Substitute.For<IIdentityService>();
        var expected = new RegisterResultDto(
            true,
            new CurrentUserDto(Guid.NewGuid(), "a@b.c", "Ada", "Lovelace", ["Member"]),
            new Dictionary<string, string[]>());
        identity.RegisterMemberAsync("a@b.c", "pw", "Ada", "Lovelace", Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await new RegisterHandler(identity)
            .Handle(new RegisterCommand("a@b.c", "pw", "Ada", "Lovelace"), CancellationToken.None);

        result.Should().Be(expected);
    }

    [Fact]
    public async Task Login_handler_returns_locked_failed_or_tokens()
    {
        var identity = Substitute.For<IIdentityService>();
        var tokens = Substitute.For<ITokenService>();
        var user = new CurrentUserDto(Guid.NewGuid(), "a@b.c", "Ada", "Lovelace", ["Member"]);

        identity.PasswordSignInAsync("a@b.c", "pw", Arg.Any<CancellationToken>())
            .Returns(new PasswordSignInResultDto(false, true, null));
        var locked = await new LoginHandler(identity, tokens)
            .Handle(new LoginCommand("a@b.c", "pw"), CancellationToken.None);
        locked.IsLockedOut.Should().BeTrue();
        locked.Succeeded.Should().BeFalse();

        identity.PasswordSignInAsync("a@b.c", "bad", Arg.Any<CancellationToken>())
            .Returns(new PasswordSignInResultDto(false, false, null));
        var failed = await new LoginHandler(identity, tokens)
            .Handle(new LoginCommand("a@b.c", "bad"), CancellationToken.None);
        failed.Succeeded.Should().BeFalse();

        var issued = new AuthTokensDto("access", "refresh", 3600, user);
        identity.PasswordSignInAsync("a@b.c", "pw", Arg.Any<CancellationToken>())
            .Returns(new PasswordSignInResultDto(true, false, user));
        tokens.IssueTokenPairAsync(user, Arg.Any<CancellationToken>()).Returns(issued);
        var ok = await new LoginHandler(identity, tokens)
            .Handle(new LoginCommand("a@b.c", "pw"), CancellationToken.None);
        ok.Succeeded.Should().BeTrue();
        ok.Tokens.Should().Be(issued);
    }

    [Fact]
    public async Task Refresh_logout_and_current_user_handlers_delegate()
    {
        var tokens = Substitute.For<ITokenService>();
        var identity = Substitute.For<IIdentityService>();
        var current = Substitute.For<ICurrentUser>();
        var user = new CurrentUserDto(Guid.NewGuid(), "a@b.c", "Ada", "Lovelace", ["Member"]);
        var pair = new AuthTokensDto("a", "b", 1, user);

        tokens.RotateAsync("old", Arg.Any<CancellationToken>()).Returns(pair);
        (await new RefreshTokenHandler(tokens).Handle(new RefreshTokenCommand("old"), CancellationToken.None))
            .Should().Be(pair);

        await new LogoutHandler(tokens).Handle(new LogoutCommand(null), CancellationToken.None);
        await tokens.DidNotReceive().RevokeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        await new LogoutHandler(tokens).Handle(new LogoutCommand("refresh"), CancellationToken.None);
        await tokens.Received(1).RevokeAsync("refresh", Arg.Any<CancellationToken>());

        current.UserId.Returns((Guid?)null);
        (await new GetCurrentUserHandler(current, identity).Handle(new GetCurrentUserQuery(), CancellationToken.None))
            .Should().BeNull();

        current.UserId.Returns(user.Id);
        identity.GetUserAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
        (await new GetCurrentUserHandler(current, identity).Handle(new GetCurrentUserQuery(), CancellationToken.None))
            .Should().Be(user);
    }
}
