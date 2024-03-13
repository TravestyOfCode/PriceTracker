using PriceTracker.Data.Results;

namespace PriceTracker.Data.Store.Commands;

public class DeleteStore : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}

internal class DeleteStoreHandler : IRequestHandler<DeleteStore, Result<Unit>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<DeleteStoreHandler> _logger;

    public DeleteStoreHandler(ApplicationDBContext dbContext, ILogger<DeleteStoreHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteStore request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Stores.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<Unit>();
            }

            _dbContext.Stores.Remove(entity);

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