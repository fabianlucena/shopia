using Microsoft.AspNetCore.Mvc.Testing;
using backend_shopia;

namespace backend_shopia_test
{
    public class TestFactory : WebApplicationFactory<Program>
    {
        public static readonly TestFactory Singleton = new();

        public TestFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }
    }
}
