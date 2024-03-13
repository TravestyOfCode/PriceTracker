using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.Product.Queries;

public class GetProductsByNameContains : IRequest<Result<List<ProductModel>>>
{
    public string Name { get; set; }

    public bool IncludeDefaultUnitOfMeasure { get; set; } = true;
}

internal class GetProductsByNameContainsHandler : IRequestHandler<GetProductsByNameContains, Result<List<ProductModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetProductsByNameContainsHandler> _logger;

    public GetProductsByNameContainsHandler(ApplicationDBContext dbContext, ILogger<GetProductsByNameContainsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<ProductModel>>> Handle(GetProductsByNameContains request, CancellationToken cancellationToken)
    {
        try
        {
            var entites = await _dbContext.Products
                .Where(p => p.Name.Contains(request.Name))
                .ProjectToModel(request.IncludeDefaultUnitOfMeasure)
                .ToListAsync(cancellationToken);

            return Result.Ok(entites);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<ProductModel>>();
        }
    }
}