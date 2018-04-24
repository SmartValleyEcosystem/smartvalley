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
                       .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmailUrl(token, false));

            await _mailSender.SendAsync(email, "Email confirmation", template);
        }

        public async Task SendUpdateEmailAsync(string address, string email)
        {
            var token = _mailTokenService.EncryptEmailConfirmationToken(address, email);
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                       .Replace("{SUBJECT}", "Please confirm email")
                       .Replace("{BODY}", $"You get this mail because you initiated email changing. Please click on the link below to confirm new email. Before this we will send all important information to the old.")
                       .Replace("{BUTTON}", "Update email")
                       .Replace("{BUTTONHREF}", _siteUrls.GetConfirmEmailUrl(token, true));

            await _mailSender.SendAsync(email, "Email changing", template);
        }

        public async Task SendOfferAsync(string email)
        {
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                       .Replace("{SUBJECT}", "Project scoring offer")
                       .Replace("{BODY}", "Click on the button below to view an offer.")
                       .Replace("{BUTTON}", "My Offers")
                       .Replace("{BUTTONHREF}", _siteUrls.GetScoringsListUrl());

            await _mailSender.SendAsync(email, "Project scoring offer", template);
        }

        public async Task SendExpertApplicationAcceptedAsync(string email, string name)
        {
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                       .Replace("{SUBJECT}", "Welcome to Smart Valley team")
                       .Replace("{BODY}", $"{name}, you're part of our expert team now. Setting your profile and choose how to get projects for scoring.")
                       .Replace("{BUTTON}", "Set profile")
                       .Replace("{BUTTONHREF}", _siteUrls.GetAccountUrl());

            await _mailSender.SendAsync(email, "Expert application approval", template);
        }

        public async Task SendExpertApplicationRejectedAsync(string email, string name)
        {
            var template = await _templateProvider.GetEmailTemplateAsync();

            template = template
                       .Replace("{SUBJECT}", "Thank you for apply")
                       .Replace("{BODY}", $"{name}, we are really sorry but we can't offer you an expert position. Your skills in specified expertise area aren't strong enough. Please choose another areas, fill the application carefully and try again. Thank you.")
                       .Replace("{BUTTON}", "Go to Smart Valley")
                       .Replace("{BUTTONHREF}", _siteUrls.GetRootUrl());

            await _mailSender.SendAsync(email, "Expert application rejection", template);
        }
    }
}