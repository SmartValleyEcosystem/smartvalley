﻿using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.WebApi.Applications.Responses;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectDetailsResponse
    {
        public string Name { get; set; }

        public string AuthorAddress { get; set; }

        public string Area { get; set; }

        public string ScoringContractAddress { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string WhitePaperLink { get; set; }

        public string BlockChainType { get; set; }

        public string Country { get; set; }

        public string FinanceModelLink { get; set; }

        public string MvpLink { get; set; }

        public string SoftCap { get; set; }

        public string HardCap { get; set; }

        public bool AttractedInvestments { get; set; }

        public double? Score { get; set; }

        public IReadOnlyCollection<TeamMemberResponse> TeamMembers { get; set; }

        public static ProjectDetailsResponse Create(ProjectDetails details)
        {
            return new ProjectDetailsResponse
                   {
                       Name = details.Project.Name,
                       Description = details.Project.Description,
                       AuthorAddress = details.Project.AuthorAddress,
                       Country = details.Project.Country,
                       Area = details.Project.ProjectArea,
                       Score = details.Scoring?.Score,
                       ScoringContractAddress = details.Scoring?.ContractAddress,
                       AttractedInvestments = details.Application.InvestmentsAreAttracted,
                       BlockChainType = details.Application.BlockchainType,
                       FinanceModelLink = details.Application.FinancialModelLink,
                       HardCap = details.Application.HardCap,
                       SoftCap = details.Application.SoftCap,
                       MvpLink = details.Application.MvpLink,
                       Status = details.Application.ProjectStatus,
                       WhitePaperLink = details.Application.WhitePaperLink,
                       TeamMembers = details.TeamMembers.Select(TeamMemberResponse.Create).ToList()
                   };
        }
    }
}