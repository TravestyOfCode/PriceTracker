using Microsoft.IdentityModel.Tokens;
using PriceTracker.Data.Product.Commands;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.Product.Behaviors;

internal class CreateValidationBehavior : IPipelineBehavior<CreateProduct, Result<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateValidationBehavior> _logger;

    public CreateValidationBehavior(ApplicationDBContext dbContext, ILogger<CreateValidationBehavior> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ProductModel>> Handle(CreateProduct request, RequestHandlerDelegate<Result<ProductModel>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = Result.BadRequest<ProductModel>();

            // Check for empty strings for Name, as IsRequired in EF allows for empty strings.
            if (request.Name.IsNullOrEmpty())
            {
                result.AddError(nameof(request.Name), $"A value for {nameof(request.Name)} is required.");
            }

            // Check for duplicate Name
            if (await _dbContext.UnitOfMeasures.AnyAsync(p => p.Name.Equals(request.Name), cancellationToken))
            {
                result.AddError(nameof(request.Name), $"The value '{request.Name}' for {nameof(request.Name)} already exists.");
            }

            // Ensure the DefaultUnitOfMeasure exists if there is one.
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
