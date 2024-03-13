using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.ComponentModel.DataAnnotations;

namespace PriceTracker.Data.Store.Commands;

public class CreateStore : IRequest<Result<StoreModel>>
{
    [Display(Name = "Name")]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(128)]
    public string Name { get; set; }
}

internal class CreateStoreHandler : IRequestHandler<CreateStore, Result<StoreModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<CreateStoreHandler> _logger;

    public CreateStoreHandler(ApplicationDBContext dbContext, ILogger<CreateStoreHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<StoreModel>> Handle(CreateStore request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.Stores.Add(new Entity.Store() { Name = request.Name });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(entity.Entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<StoreModel>();
        }
    }
}