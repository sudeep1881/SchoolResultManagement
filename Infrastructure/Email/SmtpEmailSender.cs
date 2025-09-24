using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace SchoolAttendanceManager.Infrastructure.Email
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _options;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody, string? plainTextBody = null)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            // Optional: plain text alternate (helps deliverability)
            if (!string.IsNullOrWhiteSpace(plainTextBody))
            {
                message.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(plainTextBody, null, "text/plain"));
                message.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html"));
            }

            using var client = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.UserName, _options.Password),
                EnableSsl = _options.EnableSsl
            };

            // If your server needs TLS but not SSL, Port=587 + EnableSsl=true is still correct for STARTTLS.
            await client.SendMailAsync(message);
            

        }
    }
    }
