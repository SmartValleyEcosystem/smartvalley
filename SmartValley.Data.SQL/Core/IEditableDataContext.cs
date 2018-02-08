using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public interface IEditableDataContext : IDisposable
    {
        DbSet<Application> Applications { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<EstimateComment> EstimateComments { get; set; }
        DbSet<TeamMember> TeamMembers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Voting> Votings { get; set; }
        DbSet<VotingProject> VotingProjects { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<ExpertiseArea> ExpertiseAreas { get; set; }
        DbSet<ExpertApplication> ExpertApplications { get; set; }
        DbSet<ExpertApplicationArea> ExpertApplicationAreas { get; set; }

        Task<int> SaveAsync();
        EntityEntry<T> Entity<T>(T x) where T : class;
        DbSet<T> DbSet<T>() where T : class;
    }
}