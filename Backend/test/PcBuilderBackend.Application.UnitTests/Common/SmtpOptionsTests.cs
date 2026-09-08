using FluentAssertions;
using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Application.UnitTests.Common;

public class SmtpOptionsTests
{
    [Fact]
    public void Validate_accepts_a_complete_smtp_configuration()
    {
        var options = ValidOptions();

        options.Invoking(o => o.Validate()).Should().NotThrow();
    }

    [Fact]
    public void Validate_accepts_unauthenticated_smtp()
    {
        var options = ValidOptions();
        options.User = "";
        options.Password = "";

        options.Invoking(o => o.Validate()).Should().NotThrow();
    }

    [Theory]
    [InlineData("", 587, "smtp@localhost", "user", "secret", "Smtp:Host is required.")]
    [InlineData("smtp.localhost", 0, "smtp@localhost", "user", "secret", "Smtp:Port must be between 1 and 65535.")]
    [InlineData("smtp.localhost", 587, "not-an-email", "user", "secret", "Smtp:From must be an email address.")]
    [InlineData("smtp.localhost", 587, "smtp@localhost", "user", "", "Smtp:User and Smtp:Password must both be set or both be empty.")]
    [InlineData("smtp.localhost", 587, "smtp@localhost", "", "secret", "Smtp:User and Smtp:Password must both be set or both be empty.")]
    public void Validate_rejects_incomplete_configuration(
        string host,
        int port,
        string from,
        string user,
        string password,
        string expected)
    {
        var options = new SmtpOptions
        {
            Host = host,
            Port = port,
            From = from,
            User = user,
            Password = password
        };

        options.Invoking(o => o.Validate())
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expected);
    }

    [Fact]
    public void Validate_defaults_blank_from_name()
    {
        var options = ValidOptions();
        options.FromName = "  ";

        options.Validate();

        options.FromName.Should().Be("PC Builder");
    }

    private static SmtpOptions ValidOptions() => new()
    {
        Host = "smtp.localhost",
        Port = 587,
        User = "user",
        Password = "secret",
        From = "noreply@localhost",
        FromName = "PC Builder",
        UseStartTls = true
    };
}
