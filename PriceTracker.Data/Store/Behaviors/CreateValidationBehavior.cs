using Microsoft.IdentityModel.Tokens;
using PriceTracker.Data.Results;
using PriceTracker.Data.Store.Commands;

namespace PriceTracker.Data.Store.Behaviors;

internal class CreateValidationBehavior : IPipelineBehavior<CreateStore, Result<StoreModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public CreateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<StoreModel>> Handle(CreateStore request, RequestHandlerDelegate<Result<StoreModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<StoreModel>();

            // Check for empty strings for Name, as IsRequired in EF allows for empty strings.
            if (request.Name.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Name), $"A value for {nameof(request.Name)} is required.");
            }

            // Check for duplicate Name
            if (await _dbContext.Stores.AnyAsync(p => p.Name.Equals(request.Name), cancellationToken))
            {
                result.AddError(nameof(request.Name), $"The value '{request.Name}' for {nameof(request.Name)} already exists.");
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

            return Result.ServerError<StoreModel>();
        }
    }
}
