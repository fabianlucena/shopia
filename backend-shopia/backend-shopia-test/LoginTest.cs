using System.Text;
using System.Text.Json;

namespace backend_shopia_test;

public class LoginTest
{
    private readonly HttpClient client = TestFactory.Instance.CreateClient();

    [Test]
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

        await Assert.That((int)result.StatusCode).IsEqualTo(200);
    }

    [Test]
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

        await Assert.That((int)result.StatusCode).IsEqualTo(403);
    }
}
