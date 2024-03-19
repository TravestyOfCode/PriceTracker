using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Data.UnitConversion.Behaviors;

internal static class AddBehaviorService
{
    public static MediatRServiceConfiguration AddUnitConversionBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior<CreateValidationBehavior>(ServiceLifetime.Scoped);
        config.AddBehavior<UpdateValidationBehavior>(ServiceLifetime.Scoped);

        return config;
    }
}
