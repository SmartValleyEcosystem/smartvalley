using System;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectScoringResponse
    {
        public long Id { get; set; }

        public Address ContractAddress { get; set; }

        public double? Score { get; set; }

        public ScoringStatus Status { get; set; }

        public DateTimeOffset? ScoringStartDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset OffersDueDate { get; set; }

        public DateTimeOffset? EstimatesDueDate { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public static ProjectScoringResponse Create(Scoring scoring)
        {
            return new ProjectScoringResponse
                   {
                       Id = scoring.Id,
                       ContractAddress = scoring.ContractAddress,
                       Score = scoring.Score,
                       Status = scoring.Status,
                       ScoringStartDate = scoring.ScoringStartDate,
                       CreationDate = scoring.CreationDate,
                       OffersDueDate = scoring.AcceptingDeadline,
                       EstimatesDueDate = scoring.ScoringDeadline,
                       ScoringEndDate = scoring.ScoringEndDate
            };
        }
    }
}