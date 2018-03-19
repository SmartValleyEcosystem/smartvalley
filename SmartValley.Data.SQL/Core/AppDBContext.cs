using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Core;
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

        IQueryable<ScoringOffer> IReadOnlyDataContext.ScoringOffers => ScoringOffers.AsNoTracking();

        IQueryable<AreaScoring> IReadOnlyDataContext.AreaScorings => AreaScorings.AsNoTracking();

        IQueryable<EstimateComment> IReadOnlyDataContext.EstimateComments => EstimateComments.AsNoTracking();

        IQueryable<ApplicationTeamMember> IReadOnlyDataContext.ApplicationTeamMembers => ApplicationTeamMembers.AsNoTracking();

        IQueryable<ProjectTeamMember> IReadOnlyDataContext.ProjectTeamMembers => ProjectTeamMembers.AsNoTracking();

        IQueryable<Question> IReadOnlyDataContext.Questions => Questions.AsNoTracking();

        IQueryable<Voting> IReadOnlyDataContext.Votings => Votings.AsNoTracking();

        IQueryable<VotingProject> IReadOnlyDataContext.VotingProjects => VotingProjects.AsNoTracking();

        IQueryable<User> IReadOnlyDataContext.Users => Users.AsNoTracking();

        IQueryable<Role> IReadOnlyDataContext.Roles => Roles.AsNoTracking();

        IQueryable<UserRole> IReadOnlyDataContext.UserRoles => UserRoles.AsNoTracking();

        IQueryable<Area> IReadOnlyDataContext.Areas => Areas.AsNoTracking();

        IQueryable<Expert> IReadOnlyDataContext.Experts => Experts.AsNoTracking();

        IQueryable<ExpertArea> IReadOnlyDataContext.ExpertAreas => ExpertAreas.AsNoTracking();

        IQueryable<ExpertApplication> IReadOnlyDataContext.ExpertApplications => ExpertApplications.AsNoTracking();

        IQueryable<ExpertApplicationArea> IReadOnlyDataContext.ExpertApplicationAreas => ExpertApplicationAreas.AsNoTracking();

        IQueryable<Country> IReadOnlyDataContext.Countries => Countries.AsNoTracking();

        IQueryable<Category> IReadOnlyDataContext.Categories => Categories.AsNoTracking();

        IQueryable<ProjectSocialMedia> IReadOnlyDataContext.ProjectSocialMedias => ProjectSocialMedias.AsNoTracking();

        IQueryable<SocialMedia> IReadOnlyDataContext.SocialMedias => SocialMedias.AsNoTracking();

        IQueryable<ProjectTeamMemberSocialMedia> IReadOnlyDataContext.ProjectTeamMemberSocialMedias => ProjectTeamMemberSocialMedias.AsNoTracking();

        IQueryable<ApplicationTeamMemberSocialMedia> IReadOnlyDataContext.ApplicationTeamMemberSocialMedias => ApplicationTeamMemberSocialMedias.AsNoTracking();

        IQueryable<Stage> IReadOnlyDataContext.Stages => Stages.AsNoTracking();

        public DbSet<ProjectTeamMemberSocialMedia> ProjectTeamMemberSocialMedias { get; set; }

        public DbSet<ApplicationTeamMemberSocialMedia> ApplicationTeamMemberSocialMedias { get; set; }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<ProjectSocialMedia> ProjectSocialMedias { get; set; }

        public DbSet<SocialMedia> SocialMedias { get; set; }

        public DbSet<ProjectTeamMember> ProjectTeamMembers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Scoring> Scorings { get; set; }

        public DbSet<ScoringOffer> ScoringOffers { get; set; }

        public DbSet<AreaScoring> AreaScorings { get; set; }

        public DbSet<EstimateComment> EstimateComments { get; set; }

        public DbSet<ApplicationTeamMember> ApplicationTeamMembers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Voting> Votings { get; set; }

        public DbSet<VotingProject> VotingProjects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Expert> Experts { get; set; }

        public DbSet<ExpertArea> ExpertAreas { get; set; }

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

            modelBuilder.Entity<Project>()
                        .HasIndex(p => new {p.Name});

            modelBuilder.Entity<User>()
                        .HasMany(c => c.Projects)
                        .WithOne(e => e.Author)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Scoring>()
                        .HasIndex(p => new {p.ProjectId})
                        .IsUnique();

            modelBuilder.Entity<Scoring>()
                        .HasIndex(p => new {p.ScoringEndDate});

            modelBuilder.Entity<Scoring>()
                        .HasMany(s => s.AreaScorings)
                        .WithOne(a => a.Scoring);

            modelBuilder.Entity<Scoring>()
                        .Property(b => b.ContractAddress)
                        .HasConversion(
                            v => v.ToString(),
                            v => new Address(v));

            modelBuilder.Entity<VotingProject>()
                        .HasKey(v => new {v.ProjectId, v.VotingId});

            modelBuilder.Entity<VotingProject>()
                        .HasIndex(v => new {v.ProjectId, v.VotingId});

            modelBuilder.Entity<Voting>()
                        .Property(b => b.VotingAddress)
                        .HasConversion(
                            v => v.ToString(),
                            v => new Address(v));

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Address)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .Property(b => b.Address)
                        .HasConversion(
                            v => v.ToString(),
                            v => new Address(v));

            modelBuilder.Entity<Role>()
                        .HasIndex(r => r.Name)
                        .IsUnique();

            modelBuilder.Entity<UserRole>()
                        .HasKey(u => new {u.UserId, u.RoleId});

            modelBuilder.Entity<Area>()
                        .HasKey(r => r.Id);

            modelBuilder.Entity<Area>()
                        .Property(r => r.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<Area>()
                        .HasIndex(r => r.Name)
                        .IsUnique();

            modelBuilder.Entity<ExpertApplicationArea>()
                        .HasKey(e => new {e.ExpertApplicationId, AreaType = e.AreaId});

            modelBuilder.Entity<Expert>()
                        .HasKey(r => r.UserId);

            modelBuilder.Entity<ExpertArea>()
                        .HasKey(u => new {u.ExpertId, u.AreaId});

            modelBuilder.Entity<Expert>()
                        .HasMany(c => c.ExpertAreas)
                        .WithOne(e => e.Expert)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                        .HasOne(r => r.Expert)
                        .WithOne(r => r.User)
                        .HasForeignKey<Expert>(u => u.UserId);

            modelBuilder.Entity<Stage>()
                        .HasIndex(u => u.Name)
                        .IsUnique();

            modelBuilder.Entity<SocialMedia>()
                        .HasIndex(u => u.Name)
                        .IsUnique();

            modelBuilder.Entity<Country>()
                        .HasIndex(u => u.Code)
                        .IsUnique();

            modelBuilder.Entity<Category>()
                        .HasIndex(u => u.Name)
                        .IsUnique();

            modelBuilder.Entity<ProjectSocialMedia>()
                        .HasKey(e => new {SocialId = e.SocialMediaId, e.ProjectId});

            modelBuilder.Entity<ProjectSocialMedia>()
                        .HasIndex(e => new {SocialId = e.SocialMediaId, e.ProjectId});

            modelBuilder.Entity<ProjectTeamMemberSocialMedia>()
                        .HasKey(e => new {SocialId = e.SocialMediaId, e.TeamMemberId});

            modelBuilder.Entity<ProjectTeamMemberSocialMedia>()
                        .HasIndex(e => new {SocialId = e.SocialMediaId, e.TeamMemberId});

            modelBuilder.Entity<ApplicationTeamMemberSocialMedia>()
                        .HasKey(e => new {SocialId = e.SocialMediaId, e.TeamMemberId});

            modelBuilder.Entity<ApplicationTeamMemberSocialMedia>()
                        .HasIndex(e => new {SocialId = e.SocialMediaId, e.TeamMemberId});

            modelBuilder.Entity<AreaScoring>()
                        .HasKey(e => new {e.ScoringId, e.AreaId});

            modelBuilder.Entity<AreaScoring>()
                        .HasIndex(e => new {e.ScoringId, e.AreaId});

            modelBuilder.Entity<ScoringOffer>()
                        .HasKey(e => new {e.ScoringId, e.AreaId, e.ExpertId});

            modelBuilder.Entity<ScoringOffer>()
                        .HasIndex(e => new {e.ScoringId, e.AreaId, e.ExpertId});

            modelBuilder.Entity<EstimateComment>()
                        .HasOne(x => x.Expert);
        }
    }
}