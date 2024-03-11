using PriceTracker.Data.Results;
using PriceTracker.Data.UnitOfMeasure.Commands;

namespace PriceTracker.Data.UnitOfMeasure.Behaviors;

internal class UpdateValidationBehavior : IPipelineBehavior<UpdateUnitOfMeasure, Result<UnitOfMeasureModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdateValidationBehavior> _logger;

    public UpdateValidationBehavior(ApplicationDBContext dbContext, ILogger<UpdateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitOfMeasureModel>> Handle(UpdateUnitOfMeasure request, RequestHandlerDelegate<Result<UnitOfMeasureModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<UnitOfMeasureModel>();

            // Check for duplicate Name or Abbreviation. Be sure to skip this UoM
            // when checking as we may only be updating one of the fields.
            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Name.Equals(request.Name) && !p.Id.Equals(request.Id), cancellationToken))
            {
                result.AddError(nameof(request.Name), $"The value '{request.Name}' for {nameof(request.Name)} already exists.");
            }

            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Abbreviation.Equals(request.Abbreviation) && !p.Id.Equals(request.Id), cancellationToken))
            {
                result.AddError(nameof(request.Abbreviation), $"The value '{request.Abbreviation}' for {nameof(request.Abbreviation)} already exists.");
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

            return Result.ServerError<UnitOfMeasureModel>();
        }
    }
}
