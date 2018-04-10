using System;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringOfferDetails
    {
        public ScoringOfferStatus Status { get; }

        public DateTimeOffset? ExpirationTimestamp { get; }

        public DateTimeOffset? EstimatesDueDate { get; }

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

        public double? FinalScore { get; set; }

        public ScoringOfferDetails(
            ScoringOfferStatus status, 
            DateTimeOffset? expirationTimestamp, 
            DateTimeOffset? estimatesDueDate, 
            Address scoringContractAddress, 
            long scoringId, 
            long expertId, 
            string name, 
            string countryCode, 
            Category category, 
            string description, 
            AreaType areaType, 
            Guid projectExternalId,
            long projectId,
            double? finalScore)
        {
            Status = status;
            ExpirationTimestamp = expirationTimestamp;
            EstimatesDueDate = estimatesDueDate;
            ScoringContractAddress = scoringContractAddress;
            ScoringId = scoringId;
            ExpertId = expertId;
            Name = name;
            CountryCode = countryCode;
            Category = category;
            Description = description;
            AreaType = areaType;
            ProjectExternalId = projectExternalId;
            ProjectId = projectId;
            FinalScore = finalScore;
        }
    }
}