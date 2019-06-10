using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MyIMDB.Services.Settings;

namespace MyIMDB.Services.Helpers
{
    public class EmailNotificationService : INotificationService
    {
        public EmailNotificationService()
        {
        }

        public async Task Send(NotificationMessage msg)
        {
            var message = BuildMimeMessage(msg);
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(EmailSettings.smtpHost, EmailSettings.smtpPort, EmailSettings.requiredSsl);
                await client.AuthenticateAsync(EmailSettings.adress, EmailSettings.password);
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage BuildMimeMessage(NotificationMessage message)
        {
            MimeMessage mailMessage = new MimeMessage();

            mailMessage.Sender = new MailboxAddress(EmailSettings.adress);
            mailMessage.Subject = message.Title;
            mailMessage.Body = BuildBody(message.Body);
            mailMessage.From.Add(new MailboxAddress(EmailSettings.adress));
            mailMessage.To.Add(new MailboxAddress(message.To));

            return mailMessage;
        }
        private MimeEntity BuildBody(string text)
        {
            BodyBuilder builder = new BodyBuilder
            {
                HtmlBody = text
            };
            return builder.ToMessageBody();
        }
    }
}
