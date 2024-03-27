using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;

namespace PriceTracker.Data.Product.Queries;

public class GetProductsPaged : PagedQuery<ProductModel>
{

}

internal class GetProductsPagedHandler : IRequestHandler<GetProductsPaged, PagedResult<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetProductsPagedHandler> _logger;

    public GetProductsPagedHandler(ApplicationDBContext dbContext, ILogger<GetProductsPagedHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResult<ProductModel>> Handle(GetProductsPaged request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.Products
                .AsPagedQuery(request)
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            var count = await _dbContext.Products.CountAsync(cancellationToken);

            return PagedResult<ProductModel>.Ok(entities, request, count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return PagedResult<ProductModel>.ServerError(request);
        }
    }
}