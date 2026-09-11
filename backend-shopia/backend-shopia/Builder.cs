using backend_shopia.IServices;
using backend_shopia.Services;
using RFEventBus;

namespace backend_shopia;

public static class MvcServiceCollectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddCors(options =>
        {
            options.AddPolicy("allowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()    // Permite cualquier origen (no recomendable en producción)
                           .AllowAnyMethod()    // Permite cualquier método (GET, POST, PUT, DELETE, etc.)
                           .AllowAnyHeader();   // Permite cualquier encabezado
                });
        });

        string dbConnectionString = builder.Configuration.GetConnectionString("dbConnection")
            ?? throw new Exception("No DB connection founded, try adding a dbConnection property to ConnectionStrings on appsettings.json");

        services.SetupEventBus();

        services.AddSingleton<IEmbeddingService>(provider =>
        {
            var url = builder.Configuration.GetValue<string>("Embedding:Url")
                ?? throw new Exception("No Embedding:Url configuration found");
            var apiKey = builder.Configuration.GetValue<string>("Embedding:ApiKey") ?? string.Empty;
            var model = builder.Configuration.GetValue<string>("Embedding:Model")
                ?? throw new Exception("No Embedding:Model configuration found");
            return new EmbeddingService(url, apiKey, model);
        });

        /*services.AddRFDapperDriverPostgreSQL(new PostgreSQLDDOptions
        {
            ConnectionString = dbConnectionString,
            PrepareDataSourceBuilder = ds => ds.UseVector(),
            ColumnTypes =
            {
                { "Point", property => "GEOGRAPHY(Point, 4326)" },
                { "Single[]",  property => {
                    var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                        ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength
                        ?? property.GetCustomAttribute<SizeAttribute>()?.Size;

                    if (!length.HasValue || length.Value <= 0)
                        throw new MissingLengthForPropertyException(property.Name);
                    
                    return $"Vector({length.Value})";
                } },
            },
            GetSqlSelectedProperty = (driver, property, options, defaultAlias) =>
            {
                if (property.PropertyType == typeof(Point))
                    return $"ST_AsText({driver.GetColumnName(property.Name, options, defaultAlias)}) AS {driver.GetColumnAlias(property.Name)}";

                return null;
            },
        });*/

        //SqlMapper.AddTypeHandler(new VectorFloatArrayHandler());
    }

    /*public static async Task ConfigureRepo(this WebApplication app)
    {
        var sqlFiles = app.Configuration
            .GetSection("ExecSQLFiles")
            .Get<string[]>();

        if (sqlFiles is not null)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<WebApplication>>();

            var connectionString = app.Configuration.GetConnectionString("dbConnection");

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            await using var tx = await conn.BeginTransactionAsync();

            try
            {
                foreach (var sqlFile in sqlFiles)
                {
                    logger.LogInformation($"Executing: {sqlFile}");

                    var sql = await File.ReadAllTextAsync(sqlFile);
                    await using var cmd = new NpgsqlCommand(sql, conn, tx);
                    await cmd.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
                logger.LogInformation("DB scripts executed");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error executing DB scripts: {ex.Message}");
                await tx.RollbackAsync();
                throw;
            }
        }

        if (app.Configuration.GetValue<bool>("CreateDapperTables"))
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            RFAuthDapper.Setup.ConfigureRFAuthDapper(serviceProvider);
            RFLoggerProviderDapper.Setup.ConfigureRFLoggerProviderDapper(serviceProvider);
            RFUserEmailVerifiedDapper.Setup.ConfigureRFUserEmailVerifiedDapper(serviceProvider);
            RFRBACDapper.Setup.ConfigureRFRBACDapper(serviceProvider);
            RFHttpActionDapper.Setup.ConfigureRFHttpActionDapper(serviceProvider);
            Setup.ConfigureShopiaDapper(serviceProvider);
        }
    }*/

    /*public static void ConfigureData(this WebApplication app)
    {
        if (app.Configuration.GetValue<bool>("UpdateData"))
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            RFAuth.Setup.ConfigureDataRFAuth(serviceProvider);
            RFUserEmailVerified.Setup.ConfigureDataConfigureRFUserEmailVerified(serviceProvider);
        }
    }*/
}