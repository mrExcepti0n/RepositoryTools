using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryEF.UnitTests.Helpers;
using RepositoryEF.UnitTests.Specifications;

namespace RepositoryEF.UnitTests
{
    [TestClass]
    public class ReadRepositoryTest : EntityFrameworkIntegrationBaseTest
    {
        [TestMethod]
        public void Any_EmptyCollection_ReturnsFalse()
        {
            using (var repository = new AgreementRepository(new TestDbContext()))
            {
                var result = repository.Any();
                var expected = false;

                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void SelectWithSpecification_EmptyCollection_ReturnsFalse()
        {
            string number = "12-xx-78";
            Agreement agreement = TestDbContextHelper.AddAgreement(number);

            using (var repository = new AgreementRepository(new TestDbContext()))
            {
                var specification = new NumberSpecification(number);
                var result = repository.Select(specification);

                Assert.IsNotNull(result);
            }
        }

    }
}
