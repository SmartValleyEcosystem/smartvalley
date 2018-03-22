using System;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringOfferDetails
    {
        public ScoringOfferStatus ScoringOfferStatus { get; }

        public DateTimeOffset ScoringOfferTimestamp { get; }

        public Address ScoringContractAddress { get; }

        public long ScoringId { get; }

        public long ExpertId { get; }

        public string Name { get; }

        public string CountryCode { get; }

        public string ProjectArea { get; }

        public string Description { get; }

        public AreaType AreaType { get; }

        public Guid ProjectExternalId { get; }

        public long ProjectId { get; }

        public ScoringOfferDetails(
            ScoringOfferStatus scoringOfferStatus, 
            DateTimeOffset scoringOfferTimestamp, 
            Address scoringContractAddress, 
            long scoringId, 
            long expertId, 
            string name, 
            string countryCode, 
            string projectArea, 
            string description, 
            AreaType areaType, 
            Guid projectExternalId,
            long projectId)
        {
            ScoringOfferStatus = scoringOfferStatus;
            ScoringOfferTimestamp = scoringOfferTimestamp;
            ScoringContractAddress = scoringContractAddress;
            ScoringId = scoringId;
            ExpertId = expertId;
            Name = name;
            CountryCode = countryCode;
            ProjectArea = projectArea;
            Description = description;
            AreaType = areaType;
            ProjectExternalId = projectExternalId;
            ProjectId = projectId;
        }
    }
}