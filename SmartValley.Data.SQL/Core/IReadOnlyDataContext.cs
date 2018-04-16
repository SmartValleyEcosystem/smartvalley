using System;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using ScoringApplicationQuestion = SmartValley.Domain.Entities.ScoringApplicationQuestion;

namespace SmartValley.Data.SQL.Core
{
    public interface IReadOnlyDataContext : IDisposable
    {
        IQueryable<Project> Projects { get; }
        IQueryable<Scoring> Scorings { get; }
        IQueryable<ScoringOffer> ScoringOffers { get; }
        IQueryable<AreaScoring> AreaScorings { get; }
        IQueryable<EstimateComment> EstimateComments { get; }
        IQueryable<ProjectTeamMember> ProjectTeamMembers { get; }
        IQueryable<ScoringCriterion> ScoringCriteria { get; }
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<UserRole> UserRoles { get; }
        IQueryable<Area> Areas { get; }
        IQueryable<Expert> Experts { get; }
        IQueryable<ExpertArea> ExpertAreas { get; }
        IQueryable<ExpertApplication> ExpertApplications { get; }
        IQueryable<ExpertApplicationArea> ExpertApplicationAreas { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<ScoringApplicationQuestion> ScoringApplicationQuestions { get; }
        IQueryable<ScoringApplication> ScoringApplications { get; }
        IQueryable<ScoringApplicationAnswer> ScoringApplicationAnswers { get; }
        IQueryable<ScoringApplicationTeamMember> ScoringApplicationTeamMembers { get; }
        IQueryable<ScoringApplicationAdviser> ScoringApplicationAdvisers { get; }
        IQueryable<ExpertScoringConclusion> ExpertScoringConclusions { get; }
        IQueryable<ScoringCriteriaMapping> ScoringCriteriaMappings { get; }

        IQueryable<T> GetAll<T>() where T : class;
    }
}