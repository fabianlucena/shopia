using System.Text;
using System.Text.Json;

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
            Console.WriteLine(client.BaseAddress);
        }

        [TestMethod]
        public async Task Login_returns_ok()
        {
            var body = new
            {
                username = "admin",
                password = "1234"
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/v1/login", content);

            Assert.AreEqual(200, (int)result.StatusCode);
        }

        [TestMethod]
        public async Task Login_returns_fail()
        {
            var body = new
            {
                username = "admin",
                password = "12341"
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/v1/login", content);

            Assert.AreEqual(403, (int)result.StatusCode);
        }
    }
}
