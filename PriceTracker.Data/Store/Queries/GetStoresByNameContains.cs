using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Collections.Generic;
using System.Linq;

namespace PriceTracker.Data.Store.Queries;

public class GetStoresByNameContains : IRequest<Result<List<StoreModel>>>
{
    public string Name { get; set; }
}

internal class GetStoresByNameContainsHandler : IRequestHandler<GetStoresByNameContains, Result<List<StoreModel>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetStoresByNameContainsHandler> _logger;

    public GetStoresByNameContainsHandler(ApplicationDBContext dbContext, ILogger<GetStoresByNameContainsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<StoreModel>>> Handle(GetStoresByNameContains request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Ok(await _dbContext.Stores
                .Where(p => p.Name.Contains(request.Name))
                .ProjectToModel()
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<List<StoreModel>>();
        }
    }
}