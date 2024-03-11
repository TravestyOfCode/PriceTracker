using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data.UnitOfMeasure.Behaviors;

internal static class AddBehaviorService
{
    public static MediatRServiceConfiguration AddUnitOfMeasureBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior<CreateValidationBehavior>(ServiceLifetime.Scoped);
        config.AddBehavior<UpdateValidationBehavior>(ServiceLifetime.Scoped);

        return config;
    }
}
