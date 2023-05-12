using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementations
{
    public class MailService : IEmailSender
    {
        private const string SmtpGmail = BusinessLogicConstants.SmtpGmail;
        private const int SmtpGmailPort = BusinessLogicConstants.SmtpGmailPort;
        private const string SmtpGmailLogin = BusinessLogicConstants.SmtpGmailLogin;
        private static readonly string SmtpGmailPassword = BusinessLogicConstants.SmtpGmailPassword;
        private const bool SslEnable = BusinessLogicConstants.SslEnable;
        private static readonly MailAddress _from = new(BusinessLogicConstants.SmtpGmailLogin, BusinessLogicConstants.MailAuthor);

        private readonly ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger)
        {
            this._logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                this._logger.LogInformation("Started SMTP");
                var smtp = new SmtpClient(SmtpGmail, SmtpGmailPort)
                {
                    Credentials = new NetworkCredential(SmtpGmailLogin, SmtpGmailPassword),
                    EnableSsl = SslEnable,
                };

                var toEmail = new MailAddress(to);
                var mailMessage = new MailMessage(_from, toEmail)
                {
                    Body = message,
                    BodyEncoding = Encoding.UTF8,
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mailMessage);
                mailMessage.Dispose();
                return;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception while trying to send email");
                return;
            }
        }

        public async Task SendHtmlEmailAsync(string to, string subject, string templateName, params string[] parameters)
        {
            try
            {
                var template = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "..") + templateName);

                var content = string.Format(template, parameters);

                await this.SendEmailAsync(to, subject, content);

                return;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception while trying to send email with template");
                return;
            }
        }
    }
}
