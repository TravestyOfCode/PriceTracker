using Microsoft.IdentityModel.Tokens;
using PriceTracker.Data.Product.Commands;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.Product.Behaviors;

internal class UpdateValidationBehavior : IPipelineBehavior<UpdateProduct, Result<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public UpdateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ProductModel>> Handle(UpdateProduct request, RequestHandlerDelegate<Result<ProductModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<ProductModel>();

            // Check for null/empty value for Name
            if (request.Name.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Name), $"A value for {nameof(request.Name)} is required.");
            }

            // Check for duplicate Name. Be sure to skip this Product
            // when checking as we may only be updating one of the fields.
            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Name.Equals(request.Name) && !p.Id.Equals(request.Id), cancellationToken))
            {
                result.AddError(nameof(request.Name), $"The value '{request.Name}' for {nameof(request.Name)} already exists.");
            }

            // Ensure the requested DefaultUnitOfMeasureExists
            if (request.DefaultUnitOfMeasureId != null)
            {
                if (!await _dbContext.UnitOfMeasures.AnyAsync(p => p.Id.Equals(request.DefaultUnitOfMeasureId), cancellationToken))
                {
                    result.AddError(nameof(request.DefaultUnitOfMeasureId), $"The {nameof(request.DefaultUnitOfMeasureId)} specified was not found.");
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

            return Result.ServerError<ProductModel>();
        }
    }
}
