using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Infrastructure.Services;

public sealed partial class SmtpEmailSender(SmtpOptions options, ILogger<SmtpEmailSender> logger) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(to);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.FromName, options.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = ToPlainText(htmlBody)
        }.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(options.Host, options.Port, SocketOptions(), cancellationToken);

            if (!string.IsNullOrWhiteSpace(options.User))
                await client.AuthenticateAsync(options.User, options.Password, cancellationToken);

            await client.SendAsync(message, cancellationToken);
            SmtpLog.Sent(logger, to, subject);
        }
        finally
        {
            if (client.IsConnected)
                await client.DisconnectAsync(true, cancellationToken);
        }
    }

    private SecureSocketOptions SocketOptions()
    {
        if (options.Port == 465)
            return SecureSocketOptions.SslOnConnect;

        return options.UseStartTls
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.None;
    }

    private static string ToPlainText(string html)
    {
        var withoutTags = HtmlTagRegex().Replace(html, " ");
        return WhitespaceRegex().Replace(WebUtility.HtmlDecode(withoutTags), " ").Trim();
    }

    [GeneratedRegex("<[^>]+>", RegexOptions.CultureInvariant, 1000)]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant, 1000)]
    private static partial Regex WhitespaceRegex();
}

internal static partial class SmtpLog
{
    [LoggerMessage(EventId = 3101, Level = LogLevel.Information, Message = "Sent email to {To} subject {Subject}.")]
    public static partial void Sent(ILogger logger, string to, string subject);
}
