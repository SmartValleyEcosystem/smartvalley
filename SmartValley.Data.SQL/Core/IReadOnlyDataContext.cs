using System;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public interface IReadOnlyDataContext : IDisposable
    {
        IQueryable<Application> Applications { get; }
        IQueryable<Project> Projects { get; }
        IQueryable<Scoring> Scorings { get; }
        IQueryable<EstimateComment> EstimateComments { get; }
        IQueryable<TeamMember> TeamMembers { get; }
        IQueryable<Question> Questions { get; }
        IQueryable<Voting> Votings { get; }
        IQueryable<VotingProject> VotingProjects { get; }
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<UserRole> UserRoles { get; }
        IQueryable<T> GetAll<T>() where T : class;
    }
}