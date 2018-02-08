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
        }

        IQueryable<Application> IReadOnlyDataContext.Applications => Applications.AsNoTracking();

        IQueryable<Project> IReadOnlyDataContext.Projects => Projects.AsNoTracking();

        IQueryable<Scoring> IReadOnlyDataContext.Scorings => Scorings.AsNoTracking();

        IQueryable<EstimateComment> IReadOnlyDataContext.EstimateComments => EstimateComments.AsNoTracking();

        IQueryable<TeamMember> IReadOnlyDataContext.TeamMembers => TeamMembers.AsNoTracking();

        IQueryable<Question> IReadOnlyDataContext.Questions => Questions.AsNoTracking();

        IQueryable<Voting> IReadOnlyDataContext.Votings => Votings.AsNoTracking();

        IQueryable<VotingProject> IReadOnlyDataContext.VotingProjects => VotingProjects.AsNoTracking();

        IQueryable<User> IReadOnlyDataContext.Users => Users.AsNoTracking();

        IQueryable<Role> IReadOnlyDataContext.Roles => Roles.AsNoTracking();

        IQueryable<UserRole> IReadOnlyDataContext.UserRoles => UserRoles.AsNoTracking();

        IQueryable<ExpertiseArea> IReadOnlyDataContext.ExpertiseAreas => ExpertiseAreas.AsNoTracking();

        IQueryable<ExpertApplication> IReadOnlyDataContext.ExpertApplications => ExpertApplications.AsNoTracking();

        IQueryable<ExpertApplicationArea> IReadOnlyDataContext.ExpertApplicationAreas => ExpertApplicationAreas.AsNoTracking();

        public DbSet<Application> Applications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Scoring> Scorings { get; set; }

        public DbSet<EstimateComment> EstimateComments { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Voting> Votings { get; set; }

        public DbSet<VotingProject> VotingProjects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<ExpertiseArea> ExpertiseAreas { get; set; }

        public DbSet<ExpertApplication> ExpertApplications { get; set; }

        public DbSet<ExpertApplicationArea> ExpertApplicationAreas { get; set; }

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

            modelBuilder.Entity<Scoring>()
                        .HasIndex(p => new {p.ProjectId})
                        .IsUnique();

            modelBuilder.Entity<VotingProject>()
                        .HasKey(v => new {v.ProjectId, v.VotingId});

            modelBuilder.Entity<VotingProject>()
                        .HasIndex(v => new {v.ProjectId, v.VotingId});

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Address)
                        .IsUnique();

            modelBuilder.Entity<Role>()
                        .HasIndex(r => r.Name)
                        .IsUnique();

            modelBuilder.Entity<UserRole>()
                        .HasKey(u => new {u.UserId, u.RoleId});

            modelBuilder.Entity<ExpertiseArea>()
                        .HasKey(r => r.Id);

            modelBuilder.Entity<ExpertiseArea>()
                        .Property(r => r.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<ExpertiseArea>()
                        .HasIndex(r => r.Name)
                        .IsUnique();

            modelBuilder.Entity<ExpertApplicationArea>()
                        .HasKey(e => new {e.ExpertApplicationId, ExpertiseAreaType = e.ExpertiseAreaType});
        }
    }
}