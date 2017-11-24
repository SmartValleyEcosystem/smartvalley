using System;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public interface IReadOnlyDataContext : IDisposable
    {
        IQueryable<Application> Applications { get; }
        IQueryable<Project> Projects { get; }
        IQueryable<Estimate> Estimates { get; }
        IQueryable<TeamMember> TeamMembers { get; }
        IQueryable<T> GetAll<T>() where T : class;
    }
}