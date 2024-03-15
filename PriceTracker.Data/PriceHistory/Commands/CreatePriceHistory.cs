using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.PriceHistory.Commands;

public class CreatePriceHistory : IRequest<Result<PriceHistoryModel>>
{
    [Display(Name = "Product Id")]
    public int ProductId { get; set; }

    [Display(Name = "Unit of Measure Id")]
    public int UnitOfMeasureId { get; set; }

    [Display(Name = "Quantity")]
    public decimal Quantity { get; set; }

    [Display(Name = "Date")]
    public DateTime Date { get; set; }

    [Display(Name = "Store Id")]
    public int? StoreId { get; set; }
}

internal class CreatePriceHistoryHandler : IRequestHandler<CreatePriceHistory, Result<PriceHistoryModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreatePriceHistoryHandler> _logger;

    public CreatePriceHistoryHandler(ApplicationDBContext dbContext, ILogger<CreatePriceHistoryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PriceHistoryModel>> Handle(CreatePriceHistory request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.PriceHistories.Add(new Entity.PriceHistory()
            {
                ProductId = request.ProductId,
                UnitOfMeasureId = request.UnitOfMeasureId,
                Quantity = request.Quantity,
                Date = request.Date,
                StoreId = request.StoreId
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.Entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<PriceHistoryModel>();
        }
    }
}
