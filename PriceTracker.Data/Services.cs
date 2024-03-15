using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Data.PriceHistory.Behaviors;
using PriceTracker.Data.Product.Behaviors;
using PriceTracker.Data.Store.Behaviors;
using PriceTracker.Data.UnitOfMeasure.Behaviors;
using PriceTracker.Data.User;

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

        builder.Services.AddScoped<IUserManager, UserManager>();

        // Add MediatR configuration
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            // Add behaviors
            config.AddUnitOfMeasureBehaviors()
                  .AddProductBehaviors()
                  .AddStoreBehaviors()
                  .AddPriceHistoryBehaviors();

        });

        return builder;
    }
}
