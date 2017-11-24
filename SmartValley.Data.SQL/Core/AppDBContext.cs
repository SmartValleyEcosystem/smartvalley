using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public sealed class AppDBContext : DbContext, IReadOnlyDataContext, IEditableDataContext
    {
        public AppDBContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        IQueryable<Application> IReadOnlyDataContext.Applications => Applications.AsNoTracking();

        IQueryable<Project> IReadOnlyDataContext.Projects => Projects.AsNoTracking();

        IQueryable<Estimate> IReadOnlyDataContext.Estimates => Estimates.AsNoTracking();

        IQueryable<TeamMember> IReadOnlyDataContext.TeamMembers => TeamMembers.AsNoTracking();

        public DbSet<Application> Applications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Estimate> Estimates { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public Task<int> SaveAsync()
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

        public static IEditableDataContext CreateEditable(DbContextOptions<AppDBContext> options)
        {
            return new AppDBContext(options);
        }

        public static IReadOnlyDataContext CreateReadOnly(DbContextOptions<AppDBContext> options)
        {
            var context = new AppDBContext(options);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            context.Database.AutoTransactionsEnabled = false;
            return context;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                        .HasIndex(p => new {p.ExternalId})
                        .IsUnique();
        }
    }
}