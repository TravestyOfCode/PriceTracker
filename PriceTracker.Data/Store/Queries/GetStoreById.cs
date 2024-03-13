using PriceTracker.Data.Entity;
using PriceTracker.Data.Results;
using System.Linq;

namespace PriceTracker.Data.Store.Queries;

public class GetStoreById : IRequest<Result<StoreModel>>
{
    public int Id { get; set; }
}

internal class GetStoreByIdHandler : IRequestHandler<GetStoreById, Result<StoreModel>>
{
    private readonly ApplicationDBContext _dbContext;

    private readonly ILogger<GetStoreByIdHandler> _logger;

    public GetStoreByIdHandler(ApplicationDBContext dbContext, ILogger<GetStoreByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<StoreModel>> Handle(GetStoreById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.Stores
                .Where(p => p.Id.Equals(request.Id))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return Result.NotFound<StoreModel>();
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return Result.ServerError<StoreModel>();
        }
    }
}