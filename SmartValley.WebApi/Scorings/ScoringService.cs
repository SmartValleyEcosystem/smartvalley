﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Application.Email;
using SmartValley.Domain;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Projects;
using AreaType = SmartValley.Domain.Entities.AreaType;

namespace SmartValley.WebApi.Scorings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly IScoringExpertsManagerContractClient _scoringExpertsManagerContractClient;
        private readonly MailService _mailService;
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;
        private readonly int _daysToEndScoring;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IScoringExpertsManagerContractClient scoringExpertsManagerContractClient,
            MailService mailService,
            IUserRepository userRepository,
            ScoringOptions scoringOptions,
            IClock clock)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _scoringExpertsManagerContractClient = scoringExpertsManagerContractClient;
            _mailService = mailService;
            _userRepository = userRepository;
            _clock = clock;

            _daysToEndScoring = scoringOptions.DaysToEndScoring;
        }

        public async Task<ScoringOffer> GetOfferAsync(long projectId, AreaType areaType, long expertId)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            return scoring.GetOfferForExpertinArea(expertId, areaType);
        }

        public async Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses)
        {
            var statistics = await _scoringRepository.GetIncompletedScoringAreaStatisticsAsync(_clock.UtcNow);

            var result = new List<ScoringProjectDetailsWithCounts>();
            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.Rejected))
            {
                var rejectedStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount && !i.ScoringEndDate.HasValue && i.OffersEndDate < _clock.UtcNow);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.Rejected, rejectedStatistics));
            }

            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.InProgress))
            {
                var pendingStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount && i.RequiredCount < i.AcceptedCount + i.PendingCount);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.InProgress, pendingStatistics));
            }

            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.AcceptedAndDoNotEstimate))
            {
                var acceptedAndDoNotEstimateStatistics = statistics.Where(i => i.AcceptedCount > i.FinishedCount && i.ScoringEndDate?.Date < _clock.UtcNow);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.AcceptedAndDoNotEstimate, acceptedAndDoNotEstimateStatistics));
            }

            return result;
        }

        public async Task AcceptOfferAsync(long scoringId, long areaId, long expertId)
        {
            var scoring = await _scoringRepository.GetAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            scoring.AcceptOffer((AreaType)areaId, expertId);

            var expertsAreReady = await _scoringRepository.HasEnoughExpertsAsync(scoringId);
            if (expertsAreReady)
            {
                var utcNow = _clock.UtcNow;
                scoring.EstimatesDueDate = utcNow + TimeSpan.FromDays(_daysToEndScoring);

                if (!scoring.ScoringStartDate.HasValue)
                    scoring.ScoringStartDate = utcNow;
            }

            await _scoringRepository.SaveChangesAsync();
        }

        public async Task RejectOfferAsync(long scoringId, long areaId, long expertId)
        {
            var scoring = await _scoringRepository.GetAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            scoring.RejectOffer((AreaType)areaId, expertId);
            await _scoringRepository.SaveChangesAsync();
        } 

        public async Task UpdateOffersAsync(Guid projectExternalId)
        {
            var contractOffers = await _scoringExpertsManagerContractClient.GetOffersAsync(projectExternalId);
            var offersDueDate = contractOffers.Max(i => i.ExpirationTimestamp);

            if (offersDueDate == null)
                throw new AppErrorException(ErrorCode.PendingOffersNotFound);

            var experts = await GetExpertsForOffersAsync(contractOffers);
            var expertsDictionary = experts.ToDictionary(e => e.Address, e => e.Id);

            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var scoring = await _scoringRepository.GetByProjectIdAsync(project.Id);
            var newContractOffers = contractOffers
                            .Where(o => !scoring.ScoringOffers.Any(e => e.AreaId == o.Area && e.ExpertId == expertsDictionary[o.ExpertAddress]))
                            .ToArray();

            if (!newContractOffers.Any())
                return;

            var newOffers = newContractOffers
                          .Select(o => CreateOffer(scoring.Id, ((IDictionary<Address, long>) expertsDictionary)[o.ExpertAddress], o))
                          .ToArray();

            scoring.AddOffers(newOffers);
            scoring.OffersDueDate = offersDueDate.Value;
            await _scoringRepository.SaveChangesAsync();

            await NotifyExpertsAsync(newOffers, experts);
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => _scoringRepository.GetByProjectIdAsync(projectId);

        public Task<IReadOnlyCollection<ScoringOfferDetails>> QueryOffersAsync(OffersQuery query, DateTimeOffset now)
            => _scoringOffersRepository.QueryAsync(query, now);

        public Task<int> GetOffersQueryCountAsync(OffersQuery query, DateTimeOffset now)
            => _scoringOffersRepository.GetQueryCountAsync(query, now);

        private Task<IReadOnlyCollection<User>> GetExpertsForOffersAsync(IReadOnlyCollection<ScoringOfferInfo> contractOffers)
        {
            var expertAddresses = contractOffers
                                  .Select(o => (Address) o.ExpertAddress)
                                  .Distinct()
                                  .ToArray();

            return _userRepository.GetByAddressesAsync(expertAddresses);
        }

        private Task NotifyExpertsAsync(IReadOnlyCollection<ScoringOffer> offers, IReadOnlyCollection<User> experts)
        {
            var expertEmailsDictionary = experts.ToDictionary(e => e.Id, e => e.Email);
            return Task.WhenAll(offers.Select(o => SendOfferEmailAsync(expertEmailsDictionary[o.ExpertId])));
        }

        private async Task SendOfferEmailAsync(string email)
        {
            try
            {
                await _mailService.SendOfferAsync(email);
            }
            catch (EmailSendingFailedException)
            {
                // TODO https://rassvet-capital.atlassian.net/browse/ILT-763
            }
        }

        private static ScoringOffer CreateOffer(long scoringId, long expertId, ScoringOfferInfo offerInfo)
        {
            return new ScoringOffer
                   {
                       AreaId = offerInfo.Area,
                       ExpertId = expertId,
                       ScoringId = scoringId,
                       Status = offerInfo.Status,
                       ExpirationTimestamp = offerInfo.ExpirationTimestamp
                   };
        }

        private async Task<IEnumerable<ScoringProjectDetailsWithCounts>> ConvertAreaStatisticsToProjectDetailsAsync(
            ScoringProjectStatus status,
            IEnumerable<ScoringAreaStatistics> statistics)
        {
            var areasByScoringId = statistics.ToLookup(
                o => o.ScoringId,
                k => new AreaCount(k.AreaId, k.AcceptedCount, k.RequiredCount));

            var scoringDetails = await _scoringRepository.GetScoringProjectsDetailsByScoringIdsAsync(areasByScoringId.Select(o => o.Key).ToArray());
            return scoringDetails.Select(d => CreateScoringProjectDetailsWithCounts(status, areasByScoringId[d.ScoringId].ToArray(), d));
        }

        private static ScoringProjectDetailsWithCounts CreateScoringProjectDetailsWithCounts(
            ScoringProjectStatus status,
            IReadOnlyCollection<AreaCount> areaCounts,
            ScoringProjectDetails details)
        {
            return new ScoringProjectDetailsWithCounts(
                status,
                areaCounts,
                details.ProjectId,
                details.ProjectExternalId,
                details.ScoringId,
                details.Address,
                details.Name,
                details.CreationDate,
                details.OffersEndDate);
        }
    }
}