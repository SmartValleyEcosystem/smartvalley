using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects.Responses;

namespace SmartValley.WebApi.ScoringApplications.Responses
{
    public class ProjectApplicationInfoResponse
    {
        public string Name { get; set; }

        public Category? Category { get; set; }

        public Stage? Stage { get; set; }

        public string Description { get; set; }

        public string WebSite { get; set; }

        public string CountryCode { get; set; }

        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public SocialNetworks SocialNetworks { get; set; }

        public string Articles { get; set; }

        public IEnumerable<ProjectTeamMemberResponse> ProjectTeamMembers { get; set; }

        public IEnumerable<AdviserResponse> ProjectAdvisers { get; set; }

        public static ProjectApplicationInfoResponse CreateFrom(Project project)
        {
            return new ProjectApplicationInfoResponse
                   {
                       Name = project.Name,
                       Category = project.Category,
                       Stage = project.Stage,
                       Description = project.Description,
                       CountryCode = project.Country.Code,
                       WebSite = project.Website,
                       WhitePaperLink = project.WhitePaperLink,
                       IcoDate = project.IcoDate,
                       ContactEmail = project.ContactEmail,
                       SocialNetworks = new SocialNetworks
                                        {
                                            Facebook = project.Facebook,
                                            BitcoinTalk = project.BitcoinTalk,
                                            Medium = project.Medium,
                                            Reddit = project.Reddit,
                                            Telegram = project.Telegram,
                                            Twitter = project.Twitter,
                                            Github = project.Github,
                                            Linkedin = project.Linkedin
                                        },
                       ProjectTeamMembers = project.TeamMembers.Select(ProjectTeamMemberResponse.Create)
            };
        }

        public static ProjectApplicationInfoResponse CreateFrom(Domain.ScoringApplication scoringApplication)
        {
            return new ProjectApplicationInfoResponse
                   {
                       Name = scoringApplication.ProjectName,
                       Category = scoringApplication.Category,
                       Stage = scoringApplication.Stage,
                       Description = scoringApplication.ProjectDescription,
                       CountryCode = scoringApplication.Country?.Code,
                       WebSite = scoringApplication.Site,
                       WhitePaperLink = scoringApplication.WhitePaper,
                       IcoDate = scoringApplication.IcoDate,
                       ContactEmail = scoringApplication.ContactEmail,
                       SocialNetworks = new SocialNetworks
                                        {
                                            Facebook = scoringApplication.SocialNetworks.Facebook,
                                            BitcoinTalk = scoringApplication.SocialNetworks.BitcoinTalk,
                                            Medium = scoringApplication.SocialNetworks.Medium,
                                            Reddit = scoringApplication.SocialNetworks.Reddit,
                                            Telegram = scoringApplication.SocialNetworks.Telegram,
                                            Twitter = scoringApplication.SocialNetworks.Twitter,
                                            Github = scoringApplication.SocialNetworks.Github,
                                            Linkedin = scoringApplication.SocialNetworks.Linkedin
                                        },
                       Articles = scoringApplication.Articles,
                       ProjectTeamMembers = scoringApplication.TeamMembers.Select(ProjectTeamMemberResponse.Create),
                       ProjectAdvisers = scoringApplication.Advisers.Select(a => new AdviserResponse
                                                                                 {
                                                                                     About = a.About,
                                                                                     FullName = a.FullName,
                                                                                     Reason = a.Reason,
                                                                                     FacebookLink = a.FacebookLink,
                                                                                     LinkedInLink = a.LinkedInLink
                                                                                 })
                   };
        }
    }
}