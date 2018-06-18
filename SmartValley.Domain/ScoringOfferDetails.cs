using System;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringOfferDetails
    {
        public ScoringOfferStatus Status { get; }

        public DateTimeOffset AcceptingDeadline { get; }

        public DateTimeOffset ScoringDeadline { get; }

        public Address ScoringContractAddress { get; }

        public long ScoringId { get; }

        public long ExpertId { get; }

        public string Name { get; }

        public string CountryCode { get; }

        public Category Category { get; }

        public string Description { get; }

        public AreaType AreaType { get; }

        public Guid ProjectExternalId { get; }

        public long ProjectId { get; }

        public bool IsPrivate { get; }

        public double? FinalScore { get; set; }

        public ScoringOfferDetails(
            ScoringOfferStatus status,
            DateTimeOffset acceptingDeadline,
            DateTimeOffset scoringDeadline,
            Address scoringContractAddress,
            long scoringId,
            long expertId,
            string name,
            string countryCode,
            Category category,
            string description,
            AreaType area,
            Guid projectExternalId,
            long projectId,
            bool isPrivate,
            double? finalScore)
        {
            Status = status;
            AcceptingDeadline = acceptingDeadline;
            ScoringDeadline = scoringDeadline;
            ScoringContractAddress = scoringContractAddress;
            ScoringId = scoringId;
            ExpertId = expertId;
            Name = name;
            IsPrivate = isPrivate;
            CountryCode = countryCode;
            Category = category;
            Description = description;
            AreaType = area;
            ProjectExternalId = projectExternalId;
            ProjectId = projectId;
            FinalScore = finalScore;
        }
    }
}