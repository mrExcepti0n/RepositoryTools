using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryAbstraction;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;
using System;
using System.Linq;

namespace RepositoryEF.UnitTests
{

    [TestClass]
    public class WriteTrackingEntityRepositoryTest: EntityFrameworkIntegrationBaseTest
    {
        [TestMethod]
        public void Update_InsertOneAgreementThenUpdate_SetChangeDateAndChangedBy()
        {
            var agreement = new Agreement { Date = new DateTime(2020, 05, 01), Number = "1" };
            using (var dbContext = new TestDbContext())
            {
                var repository = new AgreementWriteRepository(dbContext);

                repository.Add(agreement);
                repository.SaveChanges();
                var identityProvider = new Mock<IIdentityProvider>();
                identityProvider.Setup(a => a.User).Returns("TestUser");

                var writeTrackingEntityRepository = new WriteTrackingEntityRepository<Agreement, int>(repository, identityProvider.Object);
                writeTrackingEntityRepository.Update(a => a.Id == agreement.Id, a => new Agreement { Number = "123" });
            }

            using (var dbContext = new TestDbContext())
            {
                var updatedAgreement = dbContext.Set<Agreement>().Single(a => a.Id == agreement.Id);

                Assert.AreEqual("TestUser", updatedAgreement.ChangedBy);
                Assert.IsNotNull(updatedAgreement.ChangeDate);
                Assert.AreEqual("123", updatedAgreement.Number);
            }
        }

        [TestMethod]
        public void DeleteByExpression_InsetOneThenSoftDelete_SetIsDeleted()
        {
            var agreement = new Agreement { Date = new DateTime(2020, 05, 01), Number = "1" };
            using (var dbContext = new TestDbContext())
            {
                var repository = new AgreementWriteRepository(dbContext);

                repository.Add(agreement);
                repository.SaveChanges();
                var identityProvider = new Mock<IIdentityProvider>();
                identityProvider.Setup(a => a.User).Returns("TestUser");

                var writeTrackingEntityRepository = new WriteTrackingEntityRepository<Agreement, int>(repository, identityProvider.Object);
                writeTrackingEntityRepository.Delete(a => a.Id == agreement.Id);
            }

            using (var dbContext = new TestDbContext())
            {
                var updatedAgreement = dbContext.Set<Agreement>().Single(a => a.Id == agreement.Id);
                Assert.IsTrue(updatedAgreement.IsDeleted);
            }
        }
    }
}
