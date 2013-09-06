using NUnit.Framework;
using System.Transactions;

namespace dFrontierAppWizard.Tests
{
    public class IntegrationTestsBase
    {
        private TransactionScope scope;

        [SetUp]
        public void Initialize()
        {
            scope = new TransactionScope();
        }

        [TearDown]
        public void TestCleanup()
        {
            scope.Dispose();
        }
    }
}