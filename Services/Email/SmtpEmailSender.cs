using HotelWeb.Data;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelWeb.Services.Email;

public sealed class SmtpEmailSender : IEmailSender<ApplicationUser>
{
    private readonly SmtpOptions options;

    public SmtpEmailSender(IOptions<SmtpOptions> options)
    {
        this.options = options.Value;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendHtmlAsync(
            toEmail: email,
            subject: "Confirm your email",
            htmlBody: $"Please confirm your account by <a href=\"{confirmationLink}\">clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendHtmlAsync(
            toEmail: email,
            subject: "Reset your password",
            htmlBody: $"Please reset your password by <a href=\"{resetLink}\">clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendTextAsync(
            toEmail: email,
            subject: "Reset your password",
            textBody: $"Please reset your password using the following code: {resetCode}");

    private async Task SendHtmlAsync(string toEmail, string subject, string htmlBody)
    {
        var message = BuildMessage(toEmail, subject);
        message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();
        await SendAsync(message);
    }

    private async Task SendTextAsync(string toEmail, string subject, string textBody)
    {
        var message = BuildMessage(toEmail, subject);
        message.Body = new TextPart("plain") { Text = textBody };
        await SendAsync(message);
    }

    private MimeMessage BuildMessage(string toEmail, string subject)
    {
        if (string.IsNullOrWhiteSpace(options.Host))
            throw new InvalidOperationException("SMTP Host is not configured (Smtp:Host).");
        if (string.IsNullOrWhiteSpace(options.FromEmail))
            throw new InvalidOperationException("SMTP FromEmail is not configured (Smtp:FromEmail).");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.FromName ?? "HotelWeb", options.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        return message;
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new MailKit.Net.Smtp.SmtpClient();

        var socketOptions =
            options.UseSsl ? SecureSocketOptions.SslOnConnect :
            options.UseStartTls ? SecureSocketOptions.StartTls :
            SecureSocketOptions.None;

        await client.ConnectAsync(options.Host, options.Port, socketOptions);

        if (!string.IsNullOrWhiteSpace(options.Username))
        {
            await client.AuthenticateAsync(options.Username, options.Password);
        }

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

