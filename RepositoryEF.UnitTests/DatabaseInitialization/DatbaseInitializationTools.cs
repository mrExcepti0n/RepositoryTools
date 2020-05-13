using RepositoryEF.UnitTests.DatabaseInitialization.Configuration;
using RepositoryEF.UnitTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.UnitTests.DatabaseInitialization
{
    public static class DatbaseInitializationTools
    {
        public static void InitAndFillDatabases()
        {
            TryCreateDatabase();
        }


        private static void TryCreateDatabase()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestDbContext, TestContextConfiguration>());

            using (var ctx = new TestDbContext())
            {
                if (!ctx.Database.Exists())
                {
                    ctx.Database.Create();
                }
                else
                {
                    ctx.Database.Initialize(true);
                }
            }
        }


        public static void RemoveDatabases()
        {
            try
            {
                DatabaseContextCleaner.Clear();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
