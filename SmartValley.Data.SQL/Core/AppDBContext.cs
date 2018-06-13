using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using ScoringApplicationQuestion = SmartValley.Domain.Entities.ScoringApplicationQuestion;

namespace SmartValley.Data.SQL.Core
{
    public sealed class AppDBContext : DbContext, IReadOnlyDataContext, IEditableDataContext
    {
        public AppDBContext(DbContextOptions options)
            : base(options)
        {
        }

        IQueryable<Subscription> IReadOnlyDataContext.Subscriptions => Subscriptions.AsNoTracking();

        IQueryable<Feedback> IReadOnlyDataContext.Feedbacks => Feedbacks.AsNoTracking();

        IQueryable<Project> IReadOnlyDataContext.Projects => Projects.AsNoTracking();

        IQueryable<Scoring> IReadOnlyDataContext.Scorings => Scorings.AsNoTracking();

        IQueryable<ScoringOffer> IReadOnlyDataContext.ScoringOffers => ScoringOffers.AsNoTracking();

        IQueryable<AreaScoring> IReadOnlyDataContext.AreaScorings => AreaScorings.AsNoTracking();

        IQueryable<ProjectTeamMember> IReadOnlyDataContext.ProjectTeamMembers => ProjectTeamMembers.AsNoTracking();

        IQueryable<ScoringCriterion> IReadOnlyDataContext.ScoringCriteria => ScoringCriteria.AsNoTracking();

        IQueryable<User> IReadOnlyDataContext.Users => Users.AsNoTracking();

        IQueryable<Role> IReadOnlyDataContext.Roles => Roles.AsNoTracking();

        IQueryable<UserRole> IReadOnlyDataContext.UserRoles => UserRoles.AsNoTracking();

        IQueryable<Area> IReadOnlyDataContext.Areas => Areas.AsNoTracking();

        IQueryable<Expert> IReadOnlyDataContext.Experts => Experts.AsNoTracking();

        IQueryable<ExpertArea> IReadOnlyDataContext.ExpertAreas => ExpertAreas.AsNoTracking();

        IQueryable<ExpertApplication> IReadOnlyDataContext.ExpertApplications => ExpertApplications.AsNoTracking();

        IQueryable<Country> IReadOnlyDataContext.Countries => Countries.AsNoTracking();

        IQueryable<ScoringApplicationQuestion> IReadOnlyDataContext.ScoringApplicationQuestions => ScoringApplicationQuestions.AsNoTracking();

        IQueryable<ScoringApplication> IReadOnlyDataContext.ScoringApplications => ScoringApplications.AsNoTracking();

        IQueryable<ScoringApplicationAnswer> IReadOnlyDataContext.ScoringApplicationAnswers => ScoringApplicationAnswers.AsNoTracking();

        IQueryable<ScoringApplicationTeamMember> IReadOnlyDataContext.ScoringApplicationTeamMembers => ScoringApplicationTeamMembers.AsNoTracking();

        IQueryable<ScoringApplicationAdviser> IReadOnlyDataContext.ScoringApplicationAdvisers => ScoringApplicationAdvisers.AsNoTracking();

        IQueryable<ScoringCriteriaMapping> IReadOnlyDataContext.ScoringCriteriaMappings => ScoringCriteriaMappings.AsNoTracking();

        IQueryable<EthereumTransaction> IReadOnlyDataContext.EthereumTransactions => EthereumTransactions.AsNoTracking();

        IQueryable<AllotmentEvent> IReadOnlyDataContext.AllotmentEvents => AllotmentEvents.AsNoTracking();

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<ProjectTeamMember> ProjectTeamMembers { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Scoring> Scorings { get; set; }

        public DbSet<ScoringOffer> ScoringOffers { get; set; }

        public DbSet<AreaScoring> AreaScorings { get; set; }

        public DbSet<Estimate> Estimates { get; set; }

        public DbSet<ScoringCriterion> ScoringCriteria { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Expert> Experts { get; set; }

        public DbSet<ExpertArea> ExpertAreas { get; set; }

        public DbSet<ExpertApplication> ExpertApplications { get; set; }

        public DbSet<ExpertApplicationArea> ExpertApplicationAreas { get; set; }

        public DbSet<ScoringApplicationQuestion> ScoringApplicationQuestions { get; set; }

        public DbSet<ScoringApplication> ScoringApplications { get; set; }

        public DbSet<ScoringApplicationAnswer> ScoringApplicationAnswers { get; set; }

        public DbSet<ScoringApplicationTeamMember> ScoringApplicationTeamMembers { get; set; }

        public DbSet<ScoringApplicationAdviser> ScoringApplicationAdvisers { get; set; }

        public DbSet<ExpertScoring> ExpertScorings { get; set; }

        public DbSet<ScoringCriteriaMapping> ScoringCriteriaMappings { get; set; }

        public DbSet<EthereumTransaction> EthereumTransactions { get; set; }

        public DbSet<AllotmentEvent> AllotmentEvents { get; set; }

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

            modelBuilder.Entity<Project>()
                        .HasMany(t => t.TeamMembers);

            modelBuilder.Entity<Project>()
                        .HasOne(t => t.Scoring);

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

            modelBuilder.Entity<ExpertApplication>()
                        .HasMany(e => e.ExpertApplicationAreas)
                        .WithOne(e => e.ExpertApplication)
                        .HasForeignKey(k => k.ExpertApplicationId);

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

            modelBuilder.Entity<Country>()
                        .HasIndex(u => u.Code)
                        .IsUnique();

            modelBuilder.Entity<Country>()
                        .HasMany(i => i.ExpertApplications)
                        .WithOne()
                        .HasForeignKey(k => k.CountryId);

            modelBuilder.Entity<AreaScoring>()
                        .HasKey(e => new {e.ScoringId, e.AreaId});

            modelBuilder.Entity<AreaScoring>()
                        .HasIndex(e => new {e.ScoringId, e.AreaId});

            modelBuilder.Entity<ScoringOffer>()
                        .HasKey(e => new {e.ScoringId, e.AreaId, e.ExpertId});

            modelBuilder.Entity<ScoringOffer>()
                        .HasIndex(e => new {e.ScoringId, e.AreaId, e.ExpertId});

            modelBuilder.Entity<Scoring>()
                        .HasMany(o => o.ScoringOffers);

            modelBuilder.Entity<ScoringApplicationAnswer>()
                        .HasOne(x => x.Question);

            modelBuilder.Entity<ScoringApplication>()
                        .HasMany(x => x.Answers)
                        .WithOne(x => x.ScoringApplication)
                        .HasForeignKey(x => x.ScoringApplicationId)
                        .IsRequired();

            modelBuilder.Entity<ScoringApplication>()
                        .HasOne(x => x.Country)
                        .WithMany(x => x.ScoringApplications)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScoringApplication>()
                        .HasMany(x => x.Advisers)
                        .WithOne()
                        .IsRequired();

            modelBuilder.Entity<ScoringApplication>()
                        .HasMany(x => x.TeamMembers)
                        .WithOne()
                        .IsRequired();

            modelBuilder.Entity<ScoringApplication>()
                        .HasIndex(x => x.ProjectId)
                        .IsUnique();

            modelBuilder.Entity<ScoringApplication>()
                        .OwnsOne(x => x.SocialNetworks,
                                 sn =>
                                 {
                                     sn.Property(x => x.Github).HasColumnName("GitHubLink");
                                     sn.Property(x => x.BitcoinTalk).HasColumnName("BitcointalkLink");
                                     sn.Property(x => x.Facebook).HasColumnName("FacebookLink");
                                     sn.Property(x => x.Linkedin).HasColumnName("LinkedInLink");
                                     sn.Property(x => x.Medium).HasColumnName("MediumLink");
                                     sn.Property(x => x.Reddit).HasColumnName("RedditLink");
                                     sn.Property(x => x.Telegram).HasColumnName("TelegramLink");
                                     sn.Property(x => x.Twitter).HasColumnName("TwitterLink");
                                 });

            modelBuilder.Entity<ScoringApplication>()
                        .HasOne(x => x.ScoringStartTransaction);

            modelBuilder.Entity<ScoringApplication>()
                        .HasOne<Project>();

            modelBuilder.Entity<ScoringApplicationQuestion>()
                        .HasIndex(x => x.Key)
                        .IsUnique();

            modelBuilder.Entity<ScoringApplicationQuestion>()
                        .Property(x => x.Key)
                        .IsRequired();

            modelBuilder.Entity<ScoringApplicationQuestion>()
                        .Property(x => x.GroupKey)
                        .IsRequired();

            modelBuilder.Entity<ScoringApplicationQuestion>()
                        .Property(x => x.GroupOrder)
                        .IsRequired();

            modelBuilder.Entity<ExpertScoring>()
                        .HasKey(x => x.Id);

            modelBuilder.Entity<ExpertScoring>()
                        .HasOne(e => e.Expert);

            modelBuilder.Entity<ExpertScoring>()
                        .HasOne(e => e.Scoring)
                        .WithMany(s => s.ExpertScorings);

            modelBuilder.Entity<Estimate>()
                        .Property(x => x.Comment)
                        .IsRequired();

            modelBuilder.Entity<EthereumTransaction>()
                        .HasKey(r => r.Id);

            modelBuilder.Entity<EthereumTransaction>()
                        .HasIndex(r => r.Hash)
                        .IsUnique();

            modelBuilder.Entity<EthereumTransaction>()
                        .Property(x => x.Hash)
                        .IsRequired();

            modelBuilder.Entity<EthereumTransaction>()
                        .HasOne(t => t.User);

            modelBuilder.Entity<Subscription>()
                        .Property(b => b.ProjectId)
                        .IsRequired();

            modelBuilder.Entity<Subscription>()
                        .Property(b => b.Name)
                        .IsRequired()
                        .HasMaxLength(200);

            modelBuilder.Entity<Subscription>()
                        .Property(b => b.Phone)
                        .HasMaxLength(50);

            modelBuilder.Entity<Subscription>()
                        .Property(b => b.Sum)
                        .IsRequired()
                        .HasMaxLength(100);

            modelBuilder.Entity<Subscription>()
                        .HasOne(t => t.Project)
                        .WithMany()
                        .HasForeignKey(i => i.ProjectId);

            modelBuilder.Entity<User>()
                        .HasOne(x => x.Country)
                        .WithMany(x => x.Users)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                        .Property(b => b.City)
                        .HasMaxLength(100);

            modelBuilder.Entity<User>()
                        .Property(b => b.LinkedInLink)
                        .HasMaxLength(400);

            modelBuilder.Entity<User>()
                        .Property(b => b.FacebookLink)
                        .HasMaxLength(400);

            modelBuilder.Entity<User>()
                        .Property(b => b.BitcointalkLink)
                        .HasMaxLength(400);

            modelBuilder.Entity<Project>()
                        .HasMany(x => x.AllotmentEvents);

            modelBuilder.Entity<AllotmentEvent>()
                        .Property(x => x.Name)
                        .IsRequired()
                        .HasMaxLength(40);

            modelBuilder.Entity<AllotmentEvent>()
                        .Property(x => x.TokenContractAddress)
                        .IsRequired();

            modelBuilder.Entity<AllotmentEvent>()
                        .Property(x => x.TokenTicker)
                        .IsRequired()
                        .HasMaxLength(6);

            modelBuilder.Entity<EthereumTransaction>()
                        .HasOne(x => x.AllotmentEvent);
        }
    }
}