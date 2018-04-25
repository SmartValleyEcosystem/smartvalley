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
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");

            areaScoring.Score = score;
        }

        public AreaScoring GetAreaScoring(AreaType areaType)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");

            return areaScoring;
        }

        public bool IsOfferAccepted(long expertId, AreaType area)
        {
            var offer = ScoringOffers.FirstOrDefault(o => o.AreaId == area && o.ExpertId == expertId);
            return offer?.Status == ScoringOfferStatus.Accepted;
        }

        public void FinishOffer(long expertId, AreaType area)
        {
            var offer = ScoringOffers.FirstOrDefault(o => o.AreaId == area && o.ExpertId == expertId);
            if (offer == null)
                throw new InvalidOperationException($"Can't find offer for area: '{area}', expertId: '{expertId}'");

            offer.Status = ScoringOfferStatus.Finished;
        }

        public ScoringOffer GetOfferForExpertinArea(long expertId, AreaType area)
        {
            return ScoringOffers.FirstOrDefault(x => x.ExpertId == expertId && x.AreaId == area);
        }

        public void AddOffers(IReadOnlyCollection<ScoringOffer> offers)
        {
            foreach (var offer in offers)
                ScoringOffers.Add(offer);
        }
    }
}