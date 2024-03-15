using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.PriceHistory.Queries;

public class GetPriceHistoriesByProductId : IRequest<Result<List<PriceHistoryModel>>>
{
    public int ProductId { get; set; }
}

internal class GetPriceHistoriesByProductIdHandler : IRequestHandler<GetPriceHistoriesByProductId, Result<List<PriceHistoryModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetPriceHistoriesByProductIdHandler> _logger;

    public GetPriceHistoriesByProductIdHandler(ApplicationDBContext dbContext, ILogger<GetPriceHistoriesByProductIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<PriceHistoryModel>>> Handle(GetPriceHistoriesByProductId request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Ok(await _dbContext.PriceHistories
                .Where(p => p.ProductId.Equals(request.ProductId))
                .ProjectToModel()
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<PriceHistoryModel>>();
        }
    }
}