namespace backend_shopia_test
{
    [TestClass]
    public class LoginTest
    {
        private HttpClient client = default!;

        [TestInitialize]
        public void TestInit()
        {
            client = TestFactory.Singleton.CreateClient();
        }

        [TestMethod]
        public async Task Login_returns_ok()
        {
            var result = await client.GetAsync("/api/login");
        }
    }
}
