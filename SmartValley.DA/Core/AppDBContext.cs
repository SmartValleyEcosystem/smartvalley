using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Core
{
    public class AppDBContext : DbContext//, IEditableDataContext, IReadOnlyDataContext
    {
        public AppDBContext(DbContextOptions options)
            : base(options)
        {
        }

        //IQueryable<ItemValue> IReadOnlyDataContext.ItemValues => ItemValues.AsNoTracking();
        //IQueryable<User> IReadOnlyDataContext.Users => Users.AsNoTracking();
        //IQueryable<EtherUsdPriceHistory> IReadOnlyDataContext.EtherUsdPriceHistories => EtherUsdPriceHistories.AsNoTracking();
        //IQueryable<Investment> IReadOnlyDataContext.Investments => Investments.AsNoTracking();
        //IQueryable<EthereumTransaction> IReadOnlyDataContext.EthereumTransactions => EthereumTransactions.AsNoTracking();

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        //}
        public DbSet<Project> Projects { get; set; }

        //public IQueryable<T> GetAll<T>() where T : class
        //{
        //    return Set<T>().AsNoTracking();
        //}

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

        //public static IEditableDataContext CreateEditable(DbContextOptions<AppDBContext> options)
        //{
        //    return new AppDBContext(options);
        //}

        //public static IReadOnlyDataContext CreateReadOnly(DbContextOptions<AppDBContext> options)
        //{
        //    var context = new AppDBContext(options);
        //    context.ChangeTracker.AutoDetectChangesEnabled = false;
        //    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        //    context.Database.AutoTransactionsEnabled = false;
        //    return context;
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<EtherUsdPriceHistory>()
        //                .HasIndex(e => e.ContractAddress);

        //    modelBuilder.Entity<Investment>()
        //                .HasIndex(i => i.BitcoinTxId)
        //                .IsUnique(true);

        //    modelBuilder.Entity<EthereumTransaction>()
        //                .HasIndex(i => i.TransactionId)
        //                .IsUnique(true);
        //}
    }
}
