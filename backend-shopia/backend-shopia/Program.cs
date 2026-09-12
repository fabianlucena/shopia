using backend_shopia.Middlewares;
using Microsoft.EntityFrameworkCore;
using RFAuth.Filters;
using RFAuth.Middlewares;
using RFHttpExceptionsL10n.Middlewares;
using RFRegisterService;

namespace backend_shopia;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
        if (builder.Environment.IsEnvironment("Test"))
        {
            builder.Configuration.AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
        }
        
        // Add services to the container.
        builder.ConfigureServices();

        AttributedServiceRegistration.LoadAllAssemblies();

        var services = builder.Services;

        services.AddAttributedServices();

        var connectionString = builder.Configuration.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());
        services.AddScoped<DbContext, AppDbContext>();

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddControllers(options =>
        {
            options.Filters.Add<AuthorizationFilter>();
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var app = builder.Build();

        app.UsePathBase("/api");
        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("allowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<EnableBodyBufferingMiddleware>();
        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseMiddleware<HttpExceptionL10nMiddleware>();
        
        app.MapControllers();

        app.Run();
    }
}
