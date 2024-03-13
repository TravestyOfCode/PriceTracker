using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.Product.Queries;

public class GetProductById : IRequest<Result<ProductModel>>
{
    public int Id { get; set; }

    public bool IncludeDefaultUnitOfMeasure { get; set; } = true;
}

internal class GetProductByIdHandler : IRequestHandler<GetProductById, Result<ProductModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(ApplicationDBContext dbContext, ILogger<GetProductByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ProductModel>> Handle(GetProductById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Products
                .Where(p => p.Id.Equals(request.Id))
                .ProjectToModel(request.IncludeDefaultUnitOfMeasure)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<ProductModel>();
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<ProductModel>();
        }
    }
}
