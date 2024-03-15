using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data.PriceHistory.Behaviors;

internal static class AddBehaviorService
{
    public static MediatRServiceConfiguration AddPriceHistoryBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior<CreateValidationBehavior>(ServiceLifetime.Scoped);
        config.AddBehavior<UpdateValidationBehavior>(ServiceLifetime.Scoped);

        return config;
    }
}
