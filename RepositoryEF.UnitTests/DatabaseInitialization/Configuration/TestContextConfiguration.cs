using RepositoryEF.UnitTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.UnitTests.DatabaseInitialization.Configuration
{
    public class TestContextConfiguration : DbMigrationsConfiguration<TestDbContext>
    {
        public TestContextConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(TestDbContext context)
        {
        }
    }
}
