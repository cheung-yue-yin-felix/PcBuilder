using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Application.Auth.Commands.ForgotPassword;

public class ForgotPasswordHandler(
    IIdentityService identityService,
    IEmailSender emailSender,
    AppOptions appOptions) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var reset = await identityService.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        if (reset is null)
            return;

        var resetUrl =
            $"{appOptions.PublicBaseUrl.TrimEnd('/')}/reset-password?email={Uri.EscapeDataString(reset.Email)}&token={Uri.EscapeDataString(reset.Token)}";

        await emailSender.SendAsync(
            reset.Email,
            "Reset your PC Builder password",
            $"""
             <p>We received a request to reset your PC Builder password.</p>
             <p><a href="{resetUrl}">Reset your password</a></p>
             <p>This link expires in 1 hour. If you did not request a reset, you can ignore this email.</p>
             """,
            cancellationToken);
    }
}
