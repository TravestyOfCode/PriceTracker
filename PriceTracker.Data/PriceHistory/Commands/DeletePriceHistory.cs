using PriceTracker.Data.Results;

namespace PriceTracker.Data.PriceHistory.Commands;

public class DeletePriceHistory : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}

internal class DeletePriceHistoryHandler : IRequestHandler<DeletePriceHistory, Result<Unit>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<DeletePriceHistoryHandler> _logger;

    public DeletePriceHistoryHandler(ApplicationDBContext dbContext, ILogger<DeletePriceHistoryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeletePriceHistory request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.PriceHistories.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<Unit>();
            }

            _dbContext.PriceHistories.Remove(entity);

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
