using PriceTracker.Data.PriceHistory.Commands;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.PriceHistory.Behaviors;

internal class CreateValidationBehavior : IPipelineBehavior<CreatePriceHistory, Result<PriceHistoryModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public CreateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PriceHistoryModel>> Handle(CreatePriceHistory request, RequestHandlerDelegate<Result<PriceHistoryModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<PriceHistoryModel>();

            // Check to ensure quantity is greater than zero
            if (request.Quantity <= 0)
            {
                result.AddError(nameof(request.Quantity), $"The value for {nameof(request.Quantity)} must be greater than zero (0).");
            }

            // Ensure the reference Id exists
            if (!await _dbContext.Products.AnyAsync(p => p.Id.Equals(request.ProductId), cancellationToken))
            {
                result.AddError(nameof(request.ProductId), $"The {nameof(request.ProductId)} specified was not found.");
            }

            if (!await _dbContext.UnitOfMeasures.AnyAsync(p => p.Id.Equals(request.UnitOfMeasureId), cancellationToken))
            {
                result.AddError(nameof(request.UnitOfMeasureId), $"The {nameof(request.UnitOfMeasureId)} specified was not found.");
            }

            if (request.StoreId != null)
            {
                if (!await _dbContext.Stores.AnyAsync(p => p.Id.Equals(request.StoreId), cancellationToken))
                {
                    result.AddError(nameof(request.StoreId), $"The {nameof(request.StoreId)} specified was not found.");
                }
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

            return Result.ServerError<PriceHistoryModel>();
        }
    }
}
