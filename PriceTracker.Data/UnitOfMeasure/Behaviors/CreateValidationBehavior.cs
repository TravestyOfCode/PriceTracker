using Microsoft.IdentityModel.Tokens;
using PriceTracker.Data.Results;
using PriceTracker.Data.UnitOfMeasure.Commands;

namespace PriceTracker.Data.UnitOfMeasure.Behaviors;

internal class CreateValidationBehavior : IPipelineBehavior<CreateUnitOfMeasure, Result<UnitOfMeasureModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public CreateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UnitOfMeasureModel>> Handle(CreateUnitOfMeasure request, RequestHandlerDelegate<Result<UnitOfMeasureModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<UnitOfMeasureModel>();

            // Check for empty strings for Name and Abbreviation, as IsRequired in EF allows for empty strings.
            if (request.Name.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Name), $"A value for {nameof(request.Name)} is required.");
            }

            if (request.Abbreviation.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Abbreviation), $"A value for {nameof(request.Abbreviation)} is required.");
            }

            // Check for duplicate Name or Abbreviation
            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Name.Equals(request.Name), cancellationToken))
            {
                result.AddError(nameof(request.Name), $"The value '{request.Name}' for {nameof(request.Name)} already exists.");
            }

            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Abbreviation.Equals(request.Abbreviation), cancellationToken))
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
