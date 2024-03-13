using Microsoft.IdentityModel.Tokens;
using PriceTracker.Data.Results;
using PriceTracker.Data.Store.Commands;

namespace PriceTracker.Data.Store.Behaviors;

internal class UpdateValidationBehavior : IPipelineBehavior<UpdateStore, Result<StoreModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public UpdateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<StoreModel>> Handle(UpdateStore request, RequestHandlerDelegate<Result<StoreModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<StoreModel>();

            // Check for null/empty value for Name
            if (request.Name.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Name), $"A value for {nameof(request.Name)} is required.");
            }

            // Check for duplicate Name. Be sure to skip this Store
            // when checking as we may only be updating one of the fields.
            if (await _dbContext.Stores.AnyAsync(p => p.Name.Equals(request.Name) && !p.Id.Equals(request.Id), cancellationToken))
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
