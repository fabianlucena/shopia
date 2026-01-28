using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace backend_shopia_test
{
    [TestClass]
    public class LoginTest
    {
        private static TestFactory factory = default!;
        private HttpClient client = default!;

        [ClassInitialize]
        public static void ClassInit(TestContext _)
        {
            factory = new TestFactory();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            factory.Dispose();
        }

        [TestInitialize]
        public void TestInit()
        {
            client = factory.CreateClient();
        }

        [TestMethod]
        public async Task Login_returns_ok()
        {
            var result = await client.GetAsync("/api/login");
        }
    }
}
