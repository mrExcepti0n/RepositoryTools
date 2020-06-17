using RepositoryEF.UnitTests.Model;
using System.Data.Entity;

namespace RepositoryEF.UnitTests.Infrastructure
{
    public class AgreementContext : DbContext
    {
        public AgreementContext() : base("SqlConnection")
        {
        }


        public DbSet<Agreement> Agreements { get; set; }

       

    }
}
