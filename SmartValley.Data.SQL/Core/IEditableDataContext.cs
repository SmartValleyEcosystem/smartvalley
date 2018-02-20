using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public interface IEditableDataContext : IDisposable
    {
        DbSet<Application> Applications { get; }
        DbSet<Project> Projects { get; }
        DbSet<Scoring> Scorings { get; }
        DbSet<AreaScoring> AreaScorings { get; }
        DbSet<EstimateComment> EstimateComments { get; }
        DbSet<TeamMember> TeamMembers { get; }
        DbSet<Question> Questions { get; }
        DbSet<Voting> Votings { get; }
        DbSet<VotingProject> VotingProjects { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<Area> Areas { get; }
        DbSet<Expert> Experts { get; }
        DbSet<ExpertArea> ExpertAreas { get; }
        DbSet<ExpertApplication> ExpertApplications { get; }
        DbSet<ExpertApplicationArea> ExpertApplicationAreas { get; }
        
        Task<int> SaveAsync();
        EntityEntry<T> Entity<T>(T x) where T : class;
        DbSet<T> DbSet<T>() where T : class;
    }
}