using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.PriceHistory.Commands;

public class UpdatePriceHistory : IRequest<Result<PriceHistoryModel>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Product Id")]
    public int ProductId { get; set; }

    [Display(Name = "Unit of Measure Id")]
    public int UnitOfMeasureId { get; set; }

    [Display(Name = "Quantity")]
    public decimal Quantity { get; set; }

    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Display(Name = "Date")]
    public DateTime Date { get; set; }

    [Display(Name = "Store Id")]
    public int? StoreId { get; set; }
}

internal class UpdatePriceHistoryHandler : IRequestHandler<UpdatePriceHistory, Result<PriceHistoryModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdatePriceHistoryHandler> _logger;

    public UpdatePriceHistoryHandler(ApplicationDBContext dbContext, ILogger<UpdatePriceHistoryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PriceHistoryModel>> Handle(UpdatePriceHistory request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.PriceHistories.AsTracking().SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<PriceHistoryModel>();
            }

            entity.ProductId = request.ProductId;
            entity.UnitOfMeasureId = request.UnitOfMeasureId;
            entity.Quantity = request.Quantity;
            entity.Price = request.Price;
            entity.Date = request.Date;
            entity.StoreId = request.StoreId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<PriceHistoryModel>();
        }
    }
}
