using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using backend_shopia;

namespace backend_shopia_test
{
    public class TestFactory : WebApplicationFactory<Program>
    {
        public TestFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Reemplazar la BDD real por la de testing
                //services.RemoveAll<DbContextOptions<MyDbContext>>();
                //services.AddDbContext<MyDbContext>(options =>
                //options.UseSqlServer("connection-string-test"));
            });
        }
    }
}
