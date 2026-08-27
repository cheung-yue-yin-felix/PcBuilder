using FluentAssertions;
using FluentValidation;
using MediatR;
using PcBuilderBackend.Application.Common.Behaviors;

namespace PcBuilderBackend.Application.UnitTests.Common;

public class ValidationBehaviorTests
{
    private sealed record Ping(string Name) : IRequest<string>;

    private sealed class PingValidator : AbstractValidator<Ping>
    {
        public PingValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    [Fact]
    public async Task Continues_when_there_are_no_validators()
    {
        var behavior = new ValidationBehavior<Ping, string>([]);

        var result = await behavior.Handle(new Ping("ok"), _ => Task.FromResult("next"), CancellationToken.None);

        result.Should().Be("next");
    }

    [Fact]
    public async Task Throws_when_validator_fails()
    {
        var behavior = new ValidationBehavior<Ping, string>([new PingValidator()]);

        var act = () => behavior.Handle(new Ping(""), _ => Task.FromResult("next"), CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Invokes_next_when_valid()
    {
        var behavior = new ValidationBehavior<Ping, string>([new PingValidator()]);

        var result = await behavior.Handle(new Ping("ok"), _ => Task.FromResult("next"), CancellationToken.None);

        result.Should().Be("next");
    }
}
