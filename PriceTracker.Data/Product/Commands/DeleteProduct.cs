using PriceTracker.Data.Results;

namespace PriceTracker.Data.Product.Commands;

public class DeleteProduct : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}

internal class DeleteProductHandler : IRequestHandler<DeleteProduct, Result<Unit>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(ApplicationDBContext dbContext, ILogger<DeleteProductHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteProduct request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<Unit>();
            }

            _dbContext.Products.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok<Unit>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<Unit>();
        }
    }
}
