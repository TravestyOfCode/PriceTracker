using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data.Product.Behaviors;

internal static class AddBehaviorService
{
    public static MediatRServiceConfiguration AddProductBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior<CreateValidationBehavior>(ServiceLifetime.Scoped);
        config.AddBehavior<UpdateValidationBehavior>(ServiceLifetime.Scoped);

        return config;
    }
}
