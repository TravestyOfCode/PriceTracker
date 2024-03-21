using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Store.Commands;

public class UpdateStore : IRequest<Result<StoreModel>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }
}

internal class UpdateStoreHandler : IRequestHandler<UpdateStore, Result<StoreModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<UpdateStoreHandler> _logger;

    public UpdateStoreHandler(ApplicationDBContext dbContext, ILogger<UpdateStoreHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<StoreModel>> Handle(UpdateStore request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Stores.AsTracking().SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<StoreModel>();
            }

            entity.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<StoreModel>();
        }
    }
}