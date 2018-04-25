using System.Threading.Tasks;
using NServiceBus;
using SmartValley.Application.Email;
using SmartValley.Domain.Interfaces;
using SmartValley.Messages.Commands;

namespace SmartValley.Application.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public class NotificationsSender : IHandleMessages<SendScoringOfferNotification>
    {
        private readonly IUserRepository _userRepository;
        private readonly MailService _mailService;

        public NotificationsSender(
            IUserRepository userRepository,
            MailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task Handle(SendScoringOfferNotification message, IMessageHandlerContext context)
        {
            var user = await _userRepository.GetByIdAsync(message.ExpertId);
            await _mailService.SendOfferAsync(user.Email);
        }
    }
}