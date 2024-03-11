using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data;

public static class Services
{
    public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder, string connectionName = "DefaultConnection")
    {
        _ = builder.Services.AddDbContext<ApplicationDBContext>(config =>
        {
            config.UseSqlServer(builder.Configuration.GetConnectionString(connectionName));
            config.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        })
            .AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.Password.RequiredLength = 8;
                config.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

        // Add MediatR configuration
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        });

        return builder;
    }
}
