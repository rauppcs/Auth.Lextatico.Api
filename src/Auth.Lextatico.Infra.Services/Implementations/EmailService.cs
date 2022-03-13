using System.Security.Authentication;
using Auth.Lextatico.Infra.Services.Interfaces;
using Auth.Lextatico.Infra.Services.Models.EmailService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Auth.Lextatico.Infra.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
            message.To.Add(new MailboxAddress(emailRequest.Name, emailRequest.Email));
            message.Subject = emailRequest.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailRequest.Body;

            message.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            smtpClient.SslProtocols = SslProtocols.Tls;

            await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

            await smtpClient.SendAsync(message);
        }
    }
}
