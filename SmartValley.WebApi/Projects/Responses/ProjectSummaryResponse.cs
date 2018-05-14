using System;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Scorings.Responses;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectSummaryResponse
    {
        public long Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string CountryCode { get; set; }

        public int StageId { get; set; }

        public Category Category { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string Website { get; set; }

        public string WhitePaperLink { get; set; }

        public string Facebook { get; set; }

        public string Telegram { get; set; }

        public string Twitter { get; set; }

        public ScoringResponse Scoring { get; set; }

        public long AuthorId { get; set; }

        public bool IsApplicationSubmitted { get; set; }

        public bool IsPrivate { get; set; }

        public string AuthorAddress { get; set; }

        public ScoringStartTransactionStatus ScoringStartTransactionStatus { get; set; }

        public string ScoringStartTransactionHash { get; set; }

        public static ProjectSummaryResponse Create(
            Project project,
            Scoring scoring,
            ScoringApplication scoringApplication)
        {
            return new ProjectSummaryResponse
                   {
                       Id = project.Id,
                       ExternalId = project.ExternalId.ToString(),
                       Name = project.Name,
                       ImageUrl = project.ImageUrl,
                       CountryCode = project.Country.Code,
                       StageId = (int) project.Stage,
                       Category = project.Category,
                       IcoDate = project.IcoDate,
                       Website = project.Website,
                       WhitePaperLink = project.WhitePaperLink,
                       Facebook = project.Facebook,
                       Telegram = project.Telegram,
                       Twitter = project.Twitter,
                       IsApplicationSubmitted = scoringApplication?.IsSubmitted ?? false,
                       Scoring = ScoringResponse.FromScoring(scoring),
                       AuthorId = project.AuthorId,
                       AuthorAddress = project.Author.Address,
                       IsPrivate = project.IsPrivate,
                       ScoringStartTransactionStatus = scoringApplication?.GetTransactionStatus() ?? ScoringStartTransactionStatus.NotSubmitted,
                       ScoringStartTransactionHash = scoringApplication?.ScoringStartTransaction?.Hash
                   };
        }
    }
}