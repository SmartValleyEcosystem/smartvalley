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

        public ICollection<ExpertScoringConclusion> ExpertScoringConclusions { get; set; }

        public IReadOnlyCollection<ExpertScoringConclusion> GetConclusionsForArea(AreaType area)
        {
            return ExpertScoringConclusions.Where(x => x.Area == area).ToArray();
        }

        public void SetExpertConclusion(long expertId, ExpertScoringConclusion conclusion)
        {
            if (!IsOfferAccepted(expertId, conclusion.Area))
            {
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);
            }

            var storedConclusion = ExpertScoringConclusions.SingleOrDefault(x => x.ExpertId == expertId);
            if (storedConclusion == null)
            {
                ExpertScoringConclusions.Add(conclusion);
            }
            else
            {
                ExpertScoringConclusions.Remove(storedConclusion);
                ExpertScoringConclusions.Add(conclusion);
            }
        }

        public void SetScoreForArea(AreaType areaType, double score)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
            {
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");
            }

            areaScoring.Score = score;
        }

        public AreaScoring GetAreaScoring(AreaType areaType)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
            {
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");
            }

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
            {
                throw new InvalidOperationException($"Can't find offer for area: '{area}', expertId: '{expertId}'");
            }

            offer.Status = ScoringOfferStatus.Finished;
        }

        public ScoringOffer GetOfferForExpertinArea(long expertId, AreaType area)
        {
            return ScoringOffers.FirstOrDefault(x => x.ExpertId == expertId && x.AreaId == area);
        }
    }
}