using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PriceTracker.Data;

internal class AppDBContext : IdentityDbContext<AppUser>
{
    public AppDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
}

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
{
    public AppDBContext CreateDbContext(string[] args)
    {
        // Create a configuration builder
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

        // Configure our Db
        var builder = new DbContextOptionsBuilder<AppDBContext>();
        builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        // Return Db with options
        return new AppDBContext(builder.Options);
    }
}
