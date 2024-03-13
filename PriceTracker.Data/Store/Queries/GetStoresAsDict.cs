using PriceTracker.Data.Results;
using System.Collections.Generic;

namespace PriceTracker.Data.Store.Queries;

public class GetStoresAsDict : IRequest<Result<Dictionary<int, string>>>
{

}

internal class GetStoresAsDictHandler : IRequestHandler<GetStoresAsDict, Result<Dictionary<int, string>>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetStoresAsDictHandler> _logger;

    public GetStoresAsDictHandler(ApplicationDBContext dbContext, ILogger<GetStoresAsDictHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Dictionary<int, string>>> Handle(GetStoresAsDict request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Ok(await _dbContext.Stores.ToDictionaryAsync(p => p.Id, p => p.Name, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<Dictionary<int, string>>();
        }
    }
}