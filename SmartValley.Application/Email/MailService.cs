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
            
            template = template
                .Replace("{SUBJECT}", "Please confirm email")
                .Replace("{BODY}", "To complete registration please click the button below.")
                .Replace("{BUTTON}", "Start work")
                .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmail(token));

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }

        public async Task SendUpdateEmailAsync(string address, string email)
        {
            var token = _mailTokenService.EncryptEmailConfirmationToken(address, email);
            var template = await _templateProvider.GetEmailTemplateAsync();
            
            template = template
                .Replace("{SUBJECT}", "Password change")
                .Replace("{BODY}", $"Click on the button below to change your email ")
                .Replace("{BUTTON}", "Update email")
                .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmail(token));
            await _mailSender.SendAsync(email, "Email changing", template);
        }

        public async Task SendOfferEmailAsync(string email)
        {
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                .Replace("{SUBJECT}", "Project scoring offer")
                .Replace("{BODY}", "Click on the button below to view an offer.")
                .Replace("{BUTTON}", "My Offers")
                .Replace("{BUTTONHREF}", null);

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }
    }
}