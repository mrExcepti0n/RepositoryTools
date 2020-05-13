using BaseEntities.Interfaces;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.UnitTests.DatabaseInitialization
{
    public static class DatabaseContextCleaner
    {
        public static void Clear(bool deleteDb = true)
        {
            using (var context = new TestDbContext())
            {
                if (deleteDb)
                {
                    context.Database.Delete();
                } else
                {
                    ClearDbSet<Agreement, int>(context.Agreements);
                    context.SaveChanges();
                }
            }
        }

        private static void ClearDbSet<T, TKey>(DbSet<T> dbSet) where T : class,IIdentityEntity<TKey>, new()
        {
            foreach (var id in dbSet.Select(a => a.Id))
            {
                var entity = new T { Id = id };
                dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
        }

    }
}
