using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PriceTracker.Data;

internal class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Entity.Product> Products { get; set; }
    public DbSet<Entity.UnitOfMeasure> UnitOfMeasures { get; set; }

    public ApplicationDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
}

internal class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
{
    public ApplicationDBContext CreateDbContext(string[] args)
    {
        // Create a configuration builder
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

        // Configure our Db
        var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
        builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        // Return Db with options
        return new ApplicationDBContext(builder.Options);
    }
}
