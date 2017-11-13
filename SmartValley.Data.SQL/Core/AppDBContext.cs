using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public sealed class AppDBContext : DbContext, IReadOnlyDataContext
    {
        public AppDBContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        IQueryable<Application> IReadOnlyDataContext.Applications => Applications.AsNoTracking();
        IQueryable<Project> IReadOnlyDataContext.Projects => Projects.AsNoTracking();
        IQueryable<Country> IReadOnlyDataContext.Countries => Countries.AsNoTracking();
        IQueryable<Person> IReadOnlyDataContext.Persons => Persons.AsNoTracking();

        public DbSet<Application> Applications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public Task<int> Save()
        {
            return SaveChangesAsync();
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

        public EntityEntry<T> Entity<T>(T x) where T : class
        {
            return Entry(x);
        }
    }
}
