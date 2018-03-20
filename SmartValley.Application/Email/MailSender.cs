using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using SmartValley.Domain.Exceptions;

namespace SmartValley.Application.Email
{
    public class MailSender
    {
        private readonly SmtpOptions _smtpOptions;

        public MailSender(SmtpOptions smtpOptions)
        {
            _smtpOptions = smtpOptions;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var message = CreateMessage(to, subject, body);

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception exception)
                {
                    throw new EmailSendingFailedException(exception.ToString());
                }
            }
        }

        private MimeMessage CreateMessage(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpOptions.UserName));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder {HtmlBody = body};
            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }
    }
}