using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;
        private readonly IAllotmentEventsManagerContractClient _allotmentEventsManagerContractClient;
        private readonly IAllotmentEventContractClient _allotmentEventContractClient;
        private readonly IUserRepository _userRepository;
        private readonly IClock _clock;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository,
                                     IAllotmentEventsManagerContractClient allotmentEventsManagerContractClient,
                                     IAllotmentEventContractClient allotmentEventContractClient,
                                     IUserRepository userRepository,
                                     IClock clock)
        {
            _allotmentEventRepository = allotmentEventRepository;
            _allotmentEventsManagerContractClient = allotmentEventsManagerContractClient;
            _allotmentEventContractClient = allotmentEventContractClient;
            _userRepository = userRepository;
            _clock = clock;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query)
            => _allotmentEventRepository.QueryAsync(query, _clock.UtcNow);

        public async Task<long> CreateAsync(
            string name,
            string tokenContractAddress,
            int tokenDecimals,
            string tokenTicker,
            long projectId,
            DateTimeOffset? finishDate)
        {
            var entity = new AllotmentEvent
                         {
                             Name = name,
                             ProjectId = projectId,
                             TokenContractAddress = tokenContractAddress,
                             TokenTicker = tokenTicker,
                             TokenDecimals = tokenDecimals,
                             Status = AllotmentEventStatus.Created,
                             FinishDate = finishDate,
                             CreatedDate = _clock.UtcNow
                         };
            _allotmentEventRepository.Add(entity);

            await _allotmentEventRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(long id)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id) ?? throw new AppErrorException(ErrorCode.AllotmentEventNotFound);
            
            if (await _allotmentEventsManagerContractClient.IsDeletedAsync(id))
            {
                _allotmentEventRepository.Remove(allotmentEvent);
                await _allotmentEventRepository.SaveChangesAsync();
                return;
            }
            
            if (allotmentEvent.GetActualStatus(_clock.UtcNow) == AllotmentEventStatus.Created)
                throw new AppErrorException(ErrorCode.CantUpdateNotPublishedAllotmentEvent);

            var allotmentEventInfo = await _allotmentEventContractClient.GetInfoAsync(allotmentEvent.EventContractAddress);

            allotmentEvent.Name = allotmentEventInfo.Name;
            allotmentEvent.TokenContractAddress = allotmentEventInfo.TokenContractAddress;
            allotmentEvent.TokenDecimals = allotmentEventInfo.TokenDecimals;
            allotmentEvent.TokenTicker = allotmentEventInfo.TokenTicker;
            allotmentEvent.FinishDate = allotmentEventInfo.FinishDate;
            allotmentEvent.StartDate = allotmentEventInfo.StartDate;
            allotmentEvent.Status = allotmentEventInfo.Status;

            var users = await _userRepository.GetByAddressesAsync(allotmentEventInfo.Participants.Select(x => x.Address).ToArray());
            var participants = allotmentEventInfo.Participants.Select(x => new AllotmentEventParticipant(x.Bid, x.Share, users.First(u => u.Address == x.Address).Id, x.IsCollected)).ToArray();
            allotmentEvent.SetParticipants(participants);

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task PublishAsync(long allotmentEventId)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(allotmentEventId);
            allotmentEvent.Status = AllotmentEventStatus.Published;
            allotmentEvent.EventContractAddress = await _allotmentEventsManagerContractClient.GetAllotmentEventContractAddressAsync(allotmentEventId);

            await _allotmentEventRepository.SaveChangesAsync();
        }
    }
}