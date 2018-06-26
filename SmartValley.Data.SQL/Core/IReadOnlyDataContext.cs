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
        IQueryable<Feedback> Feedbacks { get; }
        IQueryable<Subscription> Subscriptions { get; }
        IQueryable<Scoring> Scorings { get; }
        IQueryable<ScoringOffer> ScoringOffers { get; }
        IQueryable<AreaScoring> AreaScorings { get; }
        IQueryable<ScoringCriterion> ScoringCriteria { get; }
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<UserRole> UserRoles { get; }
        IQueryable<Area> Areas { get; }
        IQueryable<ExpertApplication> ExpertApplications { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<ScoringApplicationQuestion> ScoringApplicationQuestions { get; }
        IQueryable<ScoringApplicationAnswer> ScoringApplicationAnswers { get; }
        IQueryable<ScoringCriteriaMapping> ScoringCriteriaMappings { get; }
        IQueryable<EthereumTransaction> EthereumTransactions { get; }
    }
}