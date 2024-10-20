using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace ArtFold.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;

        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(_options.SmtpServer))
            {
                throw new ArgumentNullException(nameof(_options.SmtpServer));
            }

            if (_options.SmtpPort <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.SmtpPort), "Port must be greater than zero.");
            }

            var smtpClient = new SmtpClient(_options.SmtpServer)
            {
                Port = _options.SmtpPort,
                Credentials = new NetworkCredential(_options.SmtpUsername, _options.SmtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.FromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
