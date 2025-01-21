using Bobs_Corn_Challenge.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Corn_Challenge.DataAccess
{
    public class ChallengeDbContext: DbContext
    {
        public ChallengeDbContext()
        {

        }

        public DbSet<Corn> CornPurchases { get; set; }

        public ChallengeDbContext(DbContextOptions<ChallengeDbContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Corn>()
                .Property(c => c.ClientId)
                .IsRequired();

            modelBuilder.Entity<Corn>()
                .Property(c => c.PurchaseTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
