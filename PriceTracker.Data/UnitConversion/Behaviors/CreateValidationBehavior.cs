using PriceTracker.Data.Results;
using PriceTracker.Data.UnitConversion.Commands;

namespace PriceTracker.Data.UnitConversion.Behaviors;

internal class CreateValidationBehavior : IPipelineBehavior<CreateUnitConversion, Result<UnitConversionModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public CreateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitConversionModel>> Handle(CreateUnitConversion request, RequestHandlerDelegate<Result<UnitConversionModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<UnitConversionModel>();

            // Check for ratio greater than zero;
            if (request.ConversionRatio <= 0)
            {
                result.AddError(nameof(request.ConversionRatio), $"The value for {nameof(request.ConversionRatio)} must be greater than zero (0).");
            }

            // Check the source and destination UnitOfMeasures exist.
            if (!await _dbContext.UnitOfMeasures.AnyAsync(p => p.Id.Equals(request.SourceUnitOfMeasureId), cancellationToken))
            {
                result.AddError(nameof(request.SourceUnitOfMeasureId), $"The {nameof(request.SourceUnitOfMeasureId)} specified was not found.");
            }

            if (!await _dbContext.UnitOfMeasures.AnyAsync(p => p.Id.Equals(request.DestinationUnitOfMeasureId), cancellationToken))
            {
                result.AddError(nameof(request.DestinationUnitOfMeasureId), $"The {nameof(request.DestinationUnitOfMeasureId)} specified was not found.");
            }

            // Check that there is not already a UnitConversion for this source/destination pair.
            if (await _dbContext.UnitConversions
                .CountAsync(p => p.SourceUnitOfMeasureId.Equals(request.SourceUnitOfMeasureId) &&
                            p.DestinationUnitOfMeasureId.Equals(request.DestinationUnitOfMeasureId), cancellationToken) > 0)
            {
                result.AddError(string.Empty, $"A UnitConversion already exists for this {nameof(request.SourceUnitOfMeasureId)} and {nameof(request.DestinationUnitOfMeasureId)}.");
            }

            if (result.HasErrors)
            {
                return result;
            }

            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<UnitConversionModel>();
        }
    }
}
