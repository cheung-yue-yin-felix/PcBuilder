using FluentAssertions;
using MailKit.Security;
using PcBuilderBackend.Infrastructure.Services;

namespace PcBuilderBackend.Infrastructure.UnitTests.Services;

public class SmtpEmailSenderTests
{
    [Theory]
    [InlineData(465, false, SecureSocketOptions.SslOnConnect)]
    [InlineData(587, true, SecureSocketOptions.StartTls)]
    [InlineData(25, false, SecureSocketOptions.None)]
    public void ResolveSocketOptions_matches_port_and_starttls(
        int port,
        bool useStartTls,
        SecureSocketOptions expected)
    {
        SmtpEmailSender.ResolveSocketOptions(port, useStartTls).Should().Be(expected);
    }

    [Fact]
    public void ToPlainText_strips_tags_and_collapses_whitespace()
    {
        SmtpEmailSender.ToPlainText("<p>Hello&nbsp;<strong>world</strong></p>")
            .Should().Be("Hello world");
    }
}
