using RepositoryEF.UnitTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
