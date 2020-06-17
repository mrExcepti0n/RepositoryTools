using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryEF.UnitTests.Helpers;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;

namespace RepositoryEF.UnitTests
{
    [TestClass]
    public class TemporaryTableRepositoryTest: EntityFrameworkIntegrationBaseTest
    {
        [TestMethod]
        public void BulkInsertInTempTable_JoinAgreementOnIdentityTable_ReturnAgreement()
        {
            int agreementId = TestDbContextHelper.AddAgreement().Id;

            using (var dbContext = new TestDbContext())
            {
                var repository = new TemporaryTableRepository(dbContext);
                var collection = new List<int> { agreementId, 0, -1, -2, -3};
                var query = repository.BulkInsertInTempTable(collection);

                var queryResult = query.ToList();
                Assert.IsTrue(queryResult.Count == 5);

                var agreementRepository = new AgreementRepository(dbContext);

                var agreements = agreementRepository.QueryableSelect().Join(query, agreement => agreement.Id, identity => identity.Id,
                    (agreement, identity) => agreement).ToList();
                Assert.IsTrue(agreements.Count == 1);
            }
        }


     
    }
}
