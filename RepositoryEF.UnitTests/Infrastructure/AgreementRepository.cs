using RepositoryAbstraction;
using RepositoryEF.UnitTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.UnitTests.Infrastructure
{
    public class AgreementRepository : SelectRepository<Agreement, int>
    {
        public AgreementRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
