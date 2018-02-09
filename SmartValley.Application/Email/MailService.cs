using System.Threading.Tasks;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Application.Email
{
    public class MailService
    {
        private readonly MailSender _mailSender;
        private readonly EmailUrls _siteUrls;
        private readonly MailTokenService _mailTokenService;
        private ITemplateProvider _templateProvider;

        public MailService(SiteOptions siteOptions,
                           MailTokenService mailTokenService,
                           MailSender mailSender,
                           ITemplateProvider templateProvider)
        {
            _mailTokenService = mailTokenService;
            _mailSender = mailSender;
            _templateProvider = templateProvider;
            _siteUrls = new EmailUrls(siteOptions.Root);
        }

        public async Task SendConfirmRegistrationAsync(string address, string email)
        {
            var token = _mailTokenService.EncryptEmailConfirmationToken(address, email);
            var template = await _templateProvider.GetEmailTemplateAsync();

            //TODO https://rassvet-capital.atlassian.net/browse/ILT-605
            template = template
                       .Replace("{SUBJECT}", "Welcome to the Smart Valley!")
                       .Replace("{BODY}", "Click on the button below to complete your registration.")
                       .Replace("{BUTTON}", "COMPLETE REGISTRATION")
                       .Replace("{BUTTONHREF}", _siteUrls.GetConfirmRegistration(address, token));

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }

        public async Task SendUpdateEmailAsync(string email, string newEmail)
        {
            var token = _mailTokenService.EncryptToken(newEmail);
            var template = await _templateProvider.GetEmailTemplateAsync();

            //TODO https://rassvet-capital.atlassian.net/browse/ILT-605
            template = template
                       .Replace("{SUBJECT}", "Password change")
                       .Replace("{BODY}", $"Click on the button below to change your email {email} to {newEmail}")
                       .Replace("{BUTTON}", "Update email")
                       .Replace("{BUTTONHREF}", _siteUrls.GetChangeEmail(token));
            await _mailSender.SendAsync(email, "Email changing", template);
        }
    }
}