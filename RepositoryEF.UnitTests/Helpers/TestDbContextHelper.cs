using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;

namespace RepositoryEF.UnitTests.Helpers
{
    public static class TestDbContextHelper
    {
        public static Agreement AddAgreement(string number = "1", DateTime? date = null)
        {
            var agreement = new Agreement { Date = date ?? new DateTime(2020, 05, 01), Number = number };
            using (var dbContext = new TestDbContext())
            {
                var repository = new AgreementWriteRepository(dbContext);

                repository.Add(agreement);
                repository.SaveChanges();
            }

            return agreement;
        }
    }
}
