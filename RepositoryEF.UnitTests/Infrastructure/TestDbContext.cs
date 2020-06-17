using System.Data.Entity;
using RepositoryAbstraction.Tables;

namespace RepositoryEF.UnitTests.Infrastructure
{
    public class TestDbContext : AgreementContext
    {
        public DbSet<TemporaryIntIdentity> TemporaryIntIdentities { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemporaryIntIdentity>().ToTable("#TemporaryIntIdentity");
            base.OnModelCreating(modelBuilder);
        }
    }
}
