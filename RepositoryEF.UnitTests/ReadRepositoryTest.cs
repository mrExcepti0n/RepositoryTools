using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryEF.UnitTests.Infrastructure;
using RepositoryEF.UnitTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
