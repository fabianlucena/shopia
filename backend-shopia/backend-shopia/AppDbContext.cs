using Microsoft.EntityFrameworkCore;
using RFEntitiesEF;

namespace backend_shopia;

public class AppDbContext(DbContextOptions<AppDbContext> options) : RFDbContext(options, convertToSnakeCase: true)
{
    // public DbSet<EntityName> TableName { get; set; }
}
