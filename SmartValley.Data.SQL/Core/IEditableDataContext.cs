using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public interface IEditableDataContext : IDisposable
    {
        DbSet<Project> Projects { get; }
        DbSet<Scoring> Scorings { get; }
        DbSet<ScoringOffer> ScoringOffers { get; }
        DbSet<AreaScoring> AreaScorings { get; }
        DbSet<ProjectTeamMember> ProjectTeamMembers { get; }
        DbSet<ScoringCriterion> ScoringCriteria { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<Area> Areas { get; }
        DbSet<Expert> Experts { get; }
        DbSet<ExpertArea> ExpertAreas { get; }
        DbSet<ExpertApplication> ExpertApplications { get; }
        DbSet<ExpertApplicationArea> ExpertApplicationAreas { get; }
        DbSet<Country> Countries { get; }
        DbSet<ScoringApplication> ScoringApplications { get; }
        DbSet<ScoringApplicationAnswer> ScoringApplicationAnswers { get; }
        DbSet<ScoringApplicationTeamMember> ScoringApplicationTeamMembers { get; }
        DbSet<ScoringApplicationAdviser> ScoringApplicationAdvisers { get; }
        DbSet<EthereumTransaction> EthereumTransactions { get; }

        Task<int> SaveAsync();
        EntityEntry<T> Entity<T>(T x) where T : class;
        DbSet<T> DbSet<T>() where T : class;
    }
}