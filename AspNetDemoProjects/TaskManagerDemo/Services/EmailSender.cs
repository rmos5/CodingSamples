using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

using System.Threading.Tasks;

namespace TaskManagerDemo.Services
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Execute(Options.SendGridKey, subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("raivo.mosona@gmail.com", "Administrator"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            Response result = await client.SendEmailAsync(msg);
        }
    }
}
