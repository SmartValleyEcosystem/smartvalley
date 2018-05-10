using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartValley.Domain.Core;
using SmartValley.Domain.Exceptions;

namespace SmartValley.Domain.Entities
{
    public class Scoring : IEntityWithId
    {
        public Scoring()
        {
        }

        public Scoring(
            long projectId,
            Address contractAddress,
            DateTimeOffset creationDate,
            IReadOnlyCollection<AreaScoring> areaScorings,
            IReadOnlyCollection<ScoringOffer> scoringOffers)
        {
            ProjectId = projectId;
            ContractAddress = contractAddress;
            CreationDate = creationDate;

            AreaScorings = new List<AreaScoring>(areaScorings);
            ScoringOffers = new List<ScoringOffer>();
            ExpertScorings = new List<ExpertScoring>();

            Status = ScoringStatus.InProgress;

            AddOffers(scoringOffers);
        }

        public long Id { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(42)]
        public Address ContractAddress { get; set; }

        public double? Score { get; set; }

        public ScoringStatus Status { get; set; }

        public DateTimeOffset? ScoringStartDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset OffersDueDate { get; set; }

        public DateTimeOffset? EstimatesDueDate { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public Project Project { get; set; }

        public ICollection<AreaScoring> AreaScorings { get; set; }

        public ICollection<ScoringOffer> ScoringOffers { get; set; }

        public ICollection<ExpertScoring> ExpertScorings { get; set; }

        public IReadOnlyCollection<ExpertScoring> GetScoringForArea(AreaType area)
        {
            return ExpertScorings.Where(x => x.Area == area).ToArray();
        }

        public void SetExpertScoring(long expertId, ExpertScoring scoring)
        {
            if (!IsOfferAccepted(expertId, scoring.Area))
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);

            var storedScoring = ExpertScorings.SingleOrDefault(x => x.ExpertId == expertId);
            if (storedScoring == null)
            {
                ExpertScorings.Add(scoring);
            }
            else
            {
                ExpertScorings.Remove(storedScoring);
                ExpertScorings.Add(scoring);
            }
        }

        public void SetScoreForArea(AreaType areaType, double score)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
                throw new InvalidOperationException($"Can't find score for area: '{areaType}', scoringId: '{Id}'");

            areaScoring.Score = score;
        }

        public AreaScoring GetAreaScoring(AreaType areaType)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
                throw new InvalidOperationException($"Can't find score for area: '{areaType}', scoringId: '{Id}'");

            return areaScoring;
        }

        public bool IsOfferAccepted(long expertId, AreaType area)
        {
            var offer = ScoringOffers.FirstOrDefault(o => o.AreaId == area && o.ExpertId == expertId);
            return offer?.Status == ScoringOfferStatus.Accepted;
        }

        public void FinishOffer(long expertId, AreaType area)
        {
            SetOfferStatus(area, expertId, ScoringOfferStatus.Finished);
        }

        public void RejectOffer(long expertId, AreaType area)
        {
            SetOfferStatus(area, expertId, ScoringOfferStatus.Rejected);
        }

        public void AcceptOffer(long expertId, AreaType area, DateTimeOffset estimatesDueDate, DateTimeOffset now)
        {
            SetOfferStatus(area, expertId, ScoringOfferStatus.Accepted);
            SetOfferEstimatesDueDate(area, expertId, estimatesDueDate);

            if (!HasEnoughExperts())
                return;

            if (!ScoringStartDate.HasValue)
                ScoringStartDate = now;

            EstimatesDueDate = ScoringOffers.Max(o => o.EstimatesDueDate);
        }

        public void UpdateExpiredOffers(DateTimeOffset now)
        {
            foreach (var offer in ScoringOffers)
                offer.UpdateStatus(now);
        }

        private bool HasEnoughExperts()
        {
            foreach (var areaScoring in AreaScorings)
            {
                var areaExpertsCount = ScoringOffers.Count(o => o.AreaId == areaScoring.AreaId && o.Status == ScoringOfferStatus.Accepted);
                if (areaScoring.ExpertsCount > areaExpertsCount)
                    return false;
            }

            return true;
        }

        public ScoringOffer GetOfferForExpertinArea(long expertId, AreaType area)
        {
            return ScoringOffers.FirstOrDefault(x => x.ExpertId == expertId && x.AreaId == area);
        }

        public void AddOffers(IReadOnlyCollection<ScoringOffer> offers)
        {
            foreach (var offer in offers)
            {
                ScoringOffers.Add(offer);

                if (offer.ExpirationTimestamp > OffersDueDate)
                    OffersDueDate = offer.ExpirationTimestamp;
            }
        }

        private void SetOfferStatus(AreaType area, long expertId, ScoringOfferStatus status)
        {
            var offer = GetOffer(area, expertId);

            offer.Status = status;
        }

        private void SetOfferEstimatesDueDate(AreaType area, long expertId, DateTimeOffset estimatesDueDate)
        {
            var offer = GetOffer(area, expertId);

            offer.EstimatesDueDate = estimatesDueDate;
        }

        private ScoringOffer GetOffer(AreaType area, long expertId)
        {
            return ScoringOffers.FirstOrDefault(o => o.AreaId == area && o.ExpertId == expertId)
                   ?? throw new InvalidOperationException($"Can't find offer for area: '{area}', expertId: '{expertId}', scoringId: '{Id}'");
        }
    }
}