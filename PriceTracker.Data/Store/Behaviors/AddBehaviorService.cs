using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data.Store.Behaviors;

internal static class AddBehaviorService
{
    public static MediatRServiceConfiguration AddStoreBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior<CreateValidationBehavior>(ServiceLifetime.Scoped);
        config.AddBehavior<UpdateValidationBehavior>(ServiceLifetime.Scoped);

        return config;
    }
}
