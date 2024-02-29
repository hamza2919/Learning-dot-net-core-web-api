using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Crud_Operation_Using_EF.General.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.


        public async Task SendEmailAsync(string toEmail, string subject, string VerficationLink)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, VerficationLink, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string verificationLink, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("hamzasohail2919@gmail.com", "Web API Identity Frame Work By Hamza"),
                Subject = subject,
                PlainTextContent = "Please confirm your email address",
                HtmlContent = "<strong>Please verify your email by clicking <a href=" + verificationLink + ">here</a></strong>"
                //$"<p>Please click the following link to verify your email address: {verificationLink}</p>"
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
        
    }
}
