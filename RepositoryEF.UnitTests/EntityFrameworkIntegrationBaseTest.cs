using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RepositoryEF.UnitTests
{
    public abstract class EntityFrameworkIntegrationBaseTest
    {
        protected TransactionScope TransactionScope;


        [TestInitialize]
        public void TestSetup()
        {
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }


        [TestCleanup]

        public void TestCleanup()
        {
            TransactionScope.Dispose();
        }

    }
}
