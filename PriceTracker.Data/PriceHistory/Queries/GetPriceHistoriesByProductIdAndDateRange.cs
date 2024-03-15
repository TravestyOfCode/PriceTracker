using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.PriceHistory.Queries;

public class GetPriceHistoriesByProductIdAndDateRange : IRequest<Result<List<PriceHistoryModel>>>
{
    public int ProductId { get; set; }

    public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);

    public DateTime ToDate { get; set; } = DateTime.Now;
}

internal class GetPriceHistoriesByProductIdAndDateRangeHandler : IRequestHandler<GetPriceHistoriesByProductIdAndDateRange, Result<List<PriceHistoryModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetPriceHistoriesByProductIdAndDateRangeHandler> _logger;

    public GetPriceHistoriesByProductIdAndDateRangeHandler(ApplicationDBContext dbContext, ILogger<GetPriceHistoriesByProductIdAndDateRangeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<PriceHistoryModel>>> Handle(GetPriceHistoriesByProductIdAndDateRange request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Ok(await _dbContext.PriceHistories
                .Where(p => p.ProductId.Equals(request.ProductId) &&
                        p.Date >= request.FromDate &&
                        p.Date <= request.ToDate)
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