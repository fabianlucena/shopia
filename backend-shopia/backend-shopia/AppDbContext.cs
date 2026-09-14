using backend_shopia.Entities;
using Microsoft.EntityFrameworkCore;
using RFEntitiesEF;

namespace backend_shopia;

public class AppDbContext(DbContextOptions<AppDbContext> options) : RFDbContext(options, convertToSnakeCase: true)
{
    public DbSet<Commerce> Commerces { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<ItemStore> ItemsStores { get; set; }
}
