using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryEF.UnitTests.DatabaseInitialization;


namespace RepositoryEF.UnitTests
{
    [TestClass]
    public static class TestInitialization
    {
        [AssemblyInitialize]
        public static void AssemblySetup(TestContext context)
        {
            DatabaseInitializationTools.InitAndFillDatabases();
        }




        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            DatabaseInitializationTools.RemoveDatabases();
        }
    }
}
