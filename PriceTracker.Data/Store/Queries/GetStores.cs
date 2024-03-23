using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Data.Store.Queries;

public class GetStores : IRequest<Result<List<StoreModel>>>
{

}

internal class GetStoresHandler : IRequestHandler<GetStores, Result<List<StoreModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetStoresHandler> _logger;

    public GetStoresHandler(ApplicationDBContext dbContext, ILogger<GetStoresHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<StoreModel>>> Handle(GetStores request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.Stores
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            return Result.Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<StoreModel>>();
        }
    }
}