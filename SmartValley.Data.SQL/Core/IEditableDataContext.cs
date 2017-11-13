using System;
using System.Collections.Generic;
using System.Text;
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
        DbSet<Country> Countries { get; set; }
        DbSet<Person> Persons { get; set; }
        Task<int> Save();
        EntityEntry<T> Entity<T>(T x) where T : class;
        DbSet<T> DbSet<T>() where T : class;
    }
}
