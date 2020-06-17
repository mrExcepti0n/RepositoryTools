using RepositoryEF.UnitTests.Model;
using System.Data.Entity;

namespace RepositoryEF.UnitTests.Infrastructure
{
    public class AgreementRepository : ReadRepository<Agreement, int>
    {
        public AgreementRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
