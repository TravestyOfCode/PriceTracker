using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.PriceHistory.Queries;

public class GetPriceHistoryById : IRequest<Result<PriceHistoryModel>>
{
    public int Id { get; set; }
}

internal class GetPriceHistoryByIdHandler : IRequestHandler<GetPriceHistoryById, Result<PriceHistoryModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetPriceHistoryByIdHandler> _logger;

    public GetPriceHistoryByIdHandler(ApplicationDBContext dbContext, ILogger<GetPriceHistoryByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PriceHistoryModel>> Handle(GetPriceHistoryById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.PriceHistories
                .Where(p => p.Id.Equals(request.Id))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<PriceHistoryModel>();
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<PriceHistoryModel>();
        }
    }
}