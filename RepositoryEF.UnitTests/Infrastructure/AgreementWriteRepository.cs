using RepositoryEF.UnitTests.Model;
using System.Data.Entity;

namespace RepositoryEF.UnitTests.Infrastructure
{
    public class AgreementWriteRepository : WriteRepository<Agreement, int>
    {
        public AgreementWriteRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
