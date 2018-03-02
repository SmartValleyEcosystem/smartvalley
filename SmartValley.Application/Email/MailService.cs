using System.Threading.Tasks;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Application.Email
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MailService
    {
        private readonly MailSender _mailSender;
        private readonly EmailUrls _siteUrls;
        private readonly MailTokenService _mailTokenService;
        private readonly ITemplateProvider _templateProvider;

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
                       .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmail(address, token));

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }

        public async Task SendUpdateEmailAsync(string address, string email)
        {
            var token = _mailTokenService.EncryptEmailConfirmationToken(address, email);
            var template = await _templateProvider.GetEmailTemplateAsync();

            //TODO https://rassvet-capital.atlassian.net/browse/ILT-605
            template = template
                       .Replace("{SUBJECT}", "Password change")
                       .Replace("{BODY}", $"Click on the button below to change your email ")
                       .Replace("{BUTTON}", "Update email")
                       .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmail(address, token));
            await _mailSender.SendAsync(email, "Email changing", template);
        }

        public async Task SendOfferEmailAsync(string email)
        {
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                       .Replace("{SUBJECT}", "Project scoring offer")
                       .Replace("{BODY}", "Click on the button below to view an offer.")
                       .Replace("{BUTTON}", "My Offers")
                       .Replace("{BUTTONHREF}", null); //TODO https://rassvet-capital.atlassian.net/browse/ILT-579

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }
    }
}